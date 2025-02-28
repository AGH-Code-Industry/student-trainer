using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable
{
    public const int PLAYER_IN_RANGE_DISTANCE = 10;

    public NavMeshAgent agent;
    public Animator animator;
    public float health = 100;

    public Renderer[] renderers;
    public Material normalMat, damageMat;

    private EnemyState currentState;
    [Inject] public readonly PlayerMovementService playerMovementService;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        ChangeState(new IdleState(this));
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

    public bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, playerMovementService.PlayerPosition) < 10f;
    }

    public bool InAttackRange()
    {
        return Vector3.Distance(transform.position, playerMovementService.PlayerPosition) < 2f;
    }

    public void Attack()
    {
        Debug.Log("Enemy Punch!");
        Ray ray = new Ray(transform.position, transform.forward);
        bool inRange = Physics.Raycast(ray, out var hit, 1);
        if (!inRange) return;

        IDamageable damageComponent = hit.transform.root.GetComponent<IDamageable>();
        damageComponent?.TakeDamage(1);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            ChangeState(new DeadState(this));
        }
    }

    IEnumerator FlashDamage()
    {
        foreach (Renderer r in renderers)
        {
            r.material = damageMat;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (Renderer r in renderers)
        {
            r.material = normalMat;
        }
    }

    public void Die()
    {
        Debug.Log("Enemy is Dead!");
        agent.isStopped = true;
        animator.Play("Death");
        Destroy(gameObject, 2f);
    }
}
