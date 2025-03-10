using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable
{
    const float ANIMATION_MIX_SPEED = 0.2f;

    public GenericEnemySettings settings;
    public AnimationSet animSet;

    // Hold the transform of the actual body mesh, that can be influenced by animations
    public Transform bodyTransform;
    public Transform attackOrigin;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    private float health;

    public Renderer[] renderers;

    private EnemyState currentState;
    [Inject] public readonly PlayerService playerMovementService;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = settings.moveSpeed;
        health = settings.health;

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

    public void Attack(float damage, float range)
    {
        var direction = (playerMovementService.PlayerPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        Ray ray = new Ray(transform.position, transform.forward);
        bool inRange = Physics.Raycast(ray, out RaycastHit hit, range);
        if (!inRange) return;

        IDamageable damageComponent = hit.transform.root.GetComponent<IDamageable>();
        damageComponent?.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
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
            if(currentState.GetType() == typeof(IdleState))
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

    public void PlayAnimation(string name)
    {
        animator.CrossFade(name, ANIMATION_MIX_SPEED);
    }
}
