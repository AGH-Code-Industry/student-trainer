using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class EnemyAgent : Agent, IDamagable
{
    public bool IsAlive { get; private set; }


    [Header("References")]
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public ShowDamage showDamage;

    public ICombatant combat;
    ICombatant targetCombat;
    public Transform target;

    Rigidbody rb;

    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] LayerMask targetMask;
    [SerializeField] float combatRange = 1.5f;

    [Header("Observations")]
    public float maxDistance = 10f;

    bool isNearTarget;
    float stuckTimer = 0f;

    enum Decisions
    {
        Idle,
        FollowPlayer,
        Retreat,
        LightAttack
    }

    void Start()
    {
        combat = GetComponent<ICombatant>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        showDamage = GetComponent<ShowDamage>();

        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;
        navMeshAgent.stoppingDistance = 0.1f; // FIX: musi być mniejsze niż combatRange
        navMeshAgent.autoBraking = true;

        EnemyEvents.TakeDamage += OnDealDamage;

        navMeshAgent.avoidancePriority = Random.Range(30, 60);
    }

    public override void OnEpisodeBegin()
    {
        if(!ArenaManager.Instance.EpisodeEnded)
            ArenaManager.Instance.RequestResetAllAgents();

        StopAllCoroutines();
        currentHP = maxHP;
        target = null;
        targetCombat = null;
        IsAlive = true;


        Vector3 randomPos = new Vector3(
            Random.Range(-6f, 6f),
            0,
            Random.Range(-6f, 6f)
        );

        transform.localPosition = randomPos;
    }

    // =========================
    // TARGET DETECTION & PICK
    // =========================
    void UpdateTarget()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius
        );

        Transform bestTarget = null;
        float lowestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            if (hit.transform == transform)
                continue;

            if (!hit.TryGetComponent<ICombatant>(out var c))
                continue;

            if (c.CurrentHP <= 0)
                continue;

            var distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
            if (distanceToTarget < lowestDistance)
            {
                lowestDistance = distanceToTarget;
                bestTarget = hit.transform;
            }
        }

        if (bestTarget != null)
        {
            target = bestTarget;
            targetCombat = target.GetComponent<ICombatant>();
        }
        else
        {
            target = null;
            targetCombat = null;
        }
    }

    // =========================
    // OBSERVATIONS
    // =========================
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target != null ? 1f : 0f);

        if (target != null)
        {
            Vector3 toTarget = target.position - transform.position;
            float distance = toTarget.magnitude;

            sensor.AddObservation(Mathf.Clamp01(distance / maxDistance));
            sensor.AddObservation(Vector3.Dot(transform.forward, toTarget.normalized));
            sensor.AddObservation(targetCombat.CurrentHP / targetCombat.MaxHP);
            sensor.AddObservation(targetCombat.IsAttacking ? 1f : 0f);
            sensor.AddObservation(isNearTarget ? 1f : 0f);
        }
        else
        {
            // padding gdy brak targetu
            sensor.AddObservation(1f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        sensor.AddObservation(rb.linearVelocity.x);
        sensor.AddObservation(rb.linearVelocity.z);
        sensor.AddObservation(currentHP / maxHP);
        sensor.AddObservation(combat.IsAttacking ? 1f : 0f);
    }

    // =========================
    // ACTIONS
    // =========================
    public override void OnActionReceived(ActionBuffers actions)
    {
        UpdateTarget();

        AddReward(-0.0005f); // time penalty

        int action = actions.DiscreteActions[0];
        HandleDecision((Decisions)action);
    }

    void HandleDecision(Decisions decision)
    {
        if(!IsAlive) return;

        // brak targetu → czekaj
        if (target == null || targetCombat == null)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        // commit do ataku
        if (combat.IsAttacking)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        navMeshAgent.isStopped = false;

        float dist = Vector3.Distance(transform.position, target.position);
        isNearTarget = dist <= combatRange;

        switch (decision)
        {
            case Decisions.Idle:
                break;

            case Decisions.FollowPlayer:
                GoTowardTarget();
                break;

            case Decisions.Retreat:
                if (isNearTarget)
                    RetreatFromTarget();
                else
                    AddReward(-0.05f);
                break;

            case Decisions.LightAttack:
                if (isNearTarget)
                    combat.LightAttack(target);
                else
                    AddReward(-0.05f);
                break;
        }
    }

    // =========================
    // MOVEMENT
    // =========================
    void GoTowardTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 side = Vector3.Cross(Vector3.up, dir).normalized;

        // unikalny offset dla każdego agenta
        float sideOffset = Mathf.PerlinNoise(GetInstanceID(), Time.time) * 2f - 1f;

        Vector3 desired =
            target.position
            - dir * 1.0f
            + side * sideOffset * navMeshAgent.radius * 2f;

        if (NavMesh.SamplePosition(desired, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }

        animator.Play("Run");
    }

    void RetreatFromTarget()
    {
        Vector3 dir = (transform.position - target.position).normalized;
        Vector3 retreatPos = transform.position + dir * 2.0f;

        if (NavMesh.SamplePosition(retreatPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }

        animator.Play("Run");
    }

    // =========================
    // REWARDS
    // =========================
    public void OnDealDamage(float amount)
    {
        AddReward(amount * 0.02f);
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive)
            return;

        currentHP -= damage;
        showDamage.Show();

        AddReward(-damage * 0.01f);

        if (IsAlive && currentHP <= 0)
        {
            IsAlive = false;
            AddReward(-5f); // kara za śmierć

            StartCoroutine("DeathCorutine");

            ArenaManager.Instance.OnAgentDied(this);
        }
    }

    IEnumerator DeathCorutine()
    {
        animator.Play("Death");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    // =========================
    // ACTION MASKS
    // =========================
    public override void WriteDiscreteActionMask(IDiscreteActionMask mask)
    {
        if (combat.IsAttacking)
        {
            mask.SetActionEnabled(0, (int)Decisions.FollowPlayer, false);
            mask.SetActionEnabled(0, (int)Decisions.Retreat, false);
            mask.SetActionEnabled(0, (int)Decisions.LightAttack, false);
            return;
        }

        if (target == null)
        {
            mask.SetActionEnabled(0, (int)Decisions.FollowPlayer, false);
            mask.SetActionEnabled(0, (int)Decisions.Retreat, false);
            mask.SetActionEnabled(0, (int)Decisions.LightAttack, false);
            return;
        }

        if (!isNearTarget)
        {
            mask.SetActionEnabled(0, (int)Decisions.LightAttack, false);
            mask.SetActionEnabled(0, (int)Decisions.Retreat, false);
        }
    }

    // =========================
    // COLLISIONS
    // =========================
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            IsAlive = false;
            AddReward(-5f); // kara za śmierć

            StartCoroutine("DeathCorutine");

            ArenaManager.Instance.OnAgentDied(this);
        }
    }

    void OnDisable()
    {
        EnemyEvents.TakeDamage -= OnDealDamage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // =========================
    // DEBUG NavMesh
    // =========================
    void Update()
    {
        if (navMeshAgent != null)
        {
            //LogNavMeshStatus();
        }
    }

    void LogNavMeshStatus()
    {
        Debug.Log($"[{gameObject.name}] NavMesh Status:\n" +
            $"  enabled: {navMeshAgent.enabled}\n" +
            $"  isOnNavMesh: {navMeshAgent.isOnNavMesh}\n" +
            $"  isStopped: {navMeshAgent.isStopped}\n" +
            $"  hasPath: {navMeshAgent.hasPath}\n" +
            $"  pathPending: {navMeshAgent.pathPending}\n" +
            $"  pathStatus: {navMeshAgent.pathStatus}\n" +
            $"  remainingDistance: {navMeshAgent.remainingDistance}\n" +
            $"  stoppingDistance: {navMeshAgent.stoppingDistance}\n" +
            $"  velocity: {navMeshAgent.velocity} (magnitude: {navMeshAgent.velocity.magnitude})\n" +
            $"  desiredVelocity: {navMeshAgent.desiredVelocity}\n" +
            $"  speed: {navMeshAgent.speed}\n" +
            $"  acceleration: {navMeshAgent.acceleration}\n" +
            $"  angularSpeed: {navMeshAgent.angularSpeed}\n" +
            $"  autoBraking: {navMeshAgent.autoBraking}\n" +
            $"  autoRepath: {navMeshAgent.autoRepath}\n" +
            $"  destination: {navMeshAgent.destination}\n" +
            $"  position: {transform.position}\n" +
            $"  updatePosition: {navMeshAgent.updatePosition}\n" +
            $"  updateRotation: {navMeshAgent.updateRotation}\n" +
            $"  IsAlive: {IsAlive}\n" +
            $"  combat.IsAttacking: {combat?.IsAttacking}\n" +
            $"  target: {(target != null ? target.name : "null")}\n" +
            $"  avoidancePriority: {navMeshAgent.avoidancePriority}\n" +
            $"  radius: {navMeshAgent.radius}\n" +
            $"  height: {navMeshAgent.height}\n" +
            $"  obstacleAvoidanceType: {navMeshAgent.obstacleAvoidanceType}\n" +
            $"  Rigidbody.isKinematic: {(rb != null ? rb.isKinematic.ToString() : "null")}");
    }
}
