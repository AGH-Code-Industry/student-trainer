using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TestEnemy : EnemyBase
{
    NavMeshAgent agent;

    [SerializeField]
    StandardEnemySettings settings;

    [SerializeField]
    Transform bodyTransform;

    Animator animator;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = settings.movementSpeed;

        animator = bodyTransform.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isAlerted)
        {
            isAlerted = IsPlayerSeen();
        }
        else
        {
            // Update the destination if the player's position changes
            if (agent.destination != playerTransform.position)
                agent.SetDestination(playerTransform.position);
        }
    }

    bool IsPlayerSeen()
    {
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 dirToPlayer = playerTransform.position - transform.position;
        float angle = Vector3.Angle(bodyTransform.forward, dirToPlayer);

        return dist < settings.detectionRange && angle < settings.detectionFov;
    }
}
