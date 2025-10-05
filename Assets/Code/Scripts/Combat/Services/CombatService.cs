using UnityEngine;

public class CombatService
{
    public void ResolveAttack(Character attacker, IDamageable target, AttackSO attack, WeaponSO weapon)
    {
        float totalDamage = CalculateDamage(attacker, attack, weapon);

        if (target is Character targetCharacter)
        {
            // --- Parry check ---
            if (attack.canBeParried && targetCharacter.ActionState == CharacterActionState.Parrying)
            {
                return;
            }

            // --- Block check ---
            if (attack.canBeBlocked && targetCharacter.ActionState == CharacterActionState.Blocking)
            {
                return;
            }

            // Apply effect like stun, ...
            foreach (var effect in attack.appliedEffects)
            {
                targetCharacter.ApplyEffects(effect);
            }
        }
        // --- Normal hit
        target.TakeDamage(totalDamage);

        // Knockback
        if (target is IKnockbackable knockbackable)
        {
            knockbackable.ApplyKnockback(attacker, CalculateKnockback(attack, weapon));
        }

    }

    public float CalculateDamage(Character attacker, AttackSO attack, WeaponSO weapon)
    {
        return attacker.Stats.baseAttackDamage + attack.damage + weapon.baseDamage;
    }

    public float CalculateKnockback(AttackSO attack, WeaponSO weapon)
    {
        return attack.knockback + weapon.baseKnockback;
    }
}