using ModestTree;
using UnityEngine;

public class Enemy : Character, IDamageable, IKnockbackable
{
    public override void ApplyEffects(StatusEffectSO effect)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Character attacker, float force)
    {
        Debug.Log($"ApplyKnockback {force}");
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"take damage: {amount}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }
}