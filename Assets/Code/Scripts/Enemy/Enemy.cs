using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Experimental.Rendering.RenderGraphModule;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable
{
    const float ANIMATION_MIX_SPEED = 0.2f;
    // NavMeshAgent's stoppingDistance seems to work really weird. Setting it to 1 causes the AI to stop directly
    // at the player's position,so it needs to be offset a bit, so it doesn't ram straight into the player.
    const float STOPPING_DISTANCE_OFFSET = 0.5f;

    public GenericEnemySettings settings;
    public AnimationSet animSet;

    // Hold the transform of the actual body mesh, that can be influenced by animations
    public Transform bodyTransform;
    public Transform attackOrigin;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public ComboSystem comboSystem = null;
    [HideInInspector] public float attackRange;
    private float health;
    public Renderer[] renderers;

    private EnemyState currentState;
    [Inject] public readonly PlayerService playerMovementService;
    [Inject] public readonly InventoryService invService;

    public EnemyItemDrop[] drops;
    private Rigidbody _rig;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody>();

        agent.speed = settings.moveSpeed;
        agent.acceleration = Mathf.Floor(Mathf.Pow(settings.moveSpeed, 1.85f));

        ComboList comboList = settings.attacks;
        // Select the first combo, doesn't really matter
        string defaultCombo = comboList.combos[0].name;
        MonoBehaviour mediator = this;
        comboSystem = new ComboSystem(comboList, defaultCombo, mediator);

        float minAttackRange = comboSystem.GetMinAttackRange();
        SetAttackRange(minAttackRange);

        health = settings.health;

        Chest dropChest;
        if (TryGetComponent<Chest>(out dropChest))
            dropChest.enabled = false;

        ChangeState(new IdleState(this));
    }

    public void SetAttackRange(float newRange)
    {
        agent.stoppingDistance = newRange + STOPPING_DISTANCE_OFFSET;

        attackRange = newRange + STOPPING_DISTANCE_OFFSET;
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Attack(float damage, float range)
    {
        var direction = (playerMovementService.PlayerPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        IDamageable damageComponent;

        // If the attackOrigin object is inside an enemy, this will ensure that it still deals damage
        Collider[] overlappingColliders = Physics.OverlapSphere(attackOrigin.position, 0.25f);
        foreach (Collider col in overlappingColliders)
        {
            // Avoid dealing damage to self
            if (col.transform.root == transform.root)
                continue;

            damageComponent = col.transform.root.GetComponent<IDamageable>();
            if (damageComponent != null)
            {
                damageComponent.TakeDamage(damage);
                return;
            }
        }

        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        bool inRange = Physics.Raycast(ray, out RaycastHit hit, range);
        if (!inRange) return;

        damageComponent = hit.transform.root.GetComponent<IDamageable>();
        damageComponent?.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        // Quick solution for errors when the component is disabled
        if (this.enabled == false)
            return;

        if (currentState.GetType() == typeof(DeadState))
            return;

        StartCoroutine(FlashDamage());
        health -= amount;
        if (health <= 0)
        {
            ChangeState(new DeadState(this));
        }
        else
        {
            if (currentState.GetType() == typeof(IdleState))
            {
                ChangeState(new ChasingState(this));
            }
        }
    }

    IEnumerator FlashDamage()
    {
        Dictionary<Material, Color> materialColors = new();
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                materialColors.Add(material, material.color);
                material.color = Color.red;
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                material.color = materialColors[material];
            }
        }
    }

    public void SetChasingState()
    {
        ChangeState(new ChasingState(this));
    }

    public void PlayAnimation(string name)
    {
        animator.CrossFade(name, ANIMATION_MIX_SPEED);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentState.GetType() != typeof(ChasingState))
        {
            ChangeState(new ChasingState(this));
        }
    }
}

[System.Serializable]
public struct EnemyItemDrop
{
    public ItemPreset item;
    public int amount;

    [Range(0, 100)]
    public float dropChance;
}