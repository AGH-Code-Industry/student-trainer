using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable
{
    public const int PLAYER_IN_RANGE_DISTANCE = 10;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    public float health = 100;

    public Renderer[] renderers;

    private EnemyState currentState;
    [Inject] public readonly PlayerService playerMovementService;

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

    public IEnumerator PerformAttack()
    {
        animator.Play("Punch");
        yield return new WaitForSeconds(0.43f);
        Attack();
    }

    public void Attack()
    {
        Debug.Log("Enemy Punch!");
        Ray ray = new Ray(transform.position, transform.forward);
        bool inRange = Physics.Raycast(ray, out var hit, 1);
        if (!inRange) return;

        IDamageable damageComponent = hit.transform.root.GetComponent<IDamageable>();
        damageComponent?.TakeDamage(10);
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(FlashDamage());
        health -= amount;
        if (health <= 0)
        {
            ChangeState(new DeadState(this));
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

    public void Die()
    {
        Debug.Log("Enemy is Dead!");
        agent.isStopped = true;
        animator.Play("Death");
        Destroy(gameObject, 2f);
    }
}
