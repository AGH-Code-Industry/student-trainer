using UnityEngine;

public class SimpleCombatDummy : MonoBehaviour, ICombatant, IDamagable, ITutorial
{
    public bool IsAttacking => isAttacking;
    private bool isAttacking = false;
    public bool IsBlocking => isBlocking;
    private bool isBlocking = false;
    public float CurrentHP => currentHP;
    private float currentHP = 30;
    public float MaxHP => maxHP;
    private float maxHP = 30;
    Animator animator;
    ShowDamage showDamage;
    void Start()
    {
        animator = GetComponent<Animator>();
        showDamage = GetComponent<ShowDamage>();
    }


    public void Block()
    {
        
    }

    public bool CanBlock()
    {
        return false;
    }

    public bool CanDodge()
    {
        return false;
    }

    public bool CanHeavyAttack()
    {
        return false;
    }

    public bool CanLightAttack()
    {
        return false;
    }

    public void Dodge()
    {
    }

    public void HeavyAttack()
    {
    }

    public void LightAttack(Transform target)
    {
        transform.LookAt(target);
        animator.Play("Punch_Right");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            LightAttack(other.transform);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        EnemyEvents.TakeDamage.Invoke(damage);
        showDamage.Show();
        if(currentHP <= 0)
        {
            animator.Play("Death");
        }

    }

    public void Reset()
    {
        animator.Play("Idle");
        currentHP = maxHP;
        transform.localPosition = Vector3.zero;
    }

    [ContextMenu("SetPositionToZero")]
    public void SetPositionToZero()
    {
        transform.localPosition = Vector3.zero;
    }
}