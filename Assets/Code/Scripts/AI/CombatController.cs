using System.Collections;
using UnityEngine;

public class CombatController : MonoBehaviour, ICombatant
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

    void Start()
    {
        animator = GetComponent<Animator>();
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

     private IEnumerator AttackRoutine(Transform target)
    {
        isAttacking = true;

        transform.LookAt(target);
        animator.Play("Punch_Right");
        target.GetComponent<IDamageable>()?.TakeDamage(5);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    public void LightAttack(Transform target)
    {
        if (IsAttacking)
            return;

        StartCoroutine(AttackRoutine(target));
    }
}
