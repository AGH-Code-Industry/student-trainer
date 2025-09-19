using Combat.Interfaces;
using UnityEngine;

namespace Combat
{

    [System.Serializable]
    public class HitEffectDealDamage : HitEffect
    {
        public DamageData damageData;

        public override void Execute(ICombatant attacker, IDamagable target)
        {
            if (target == null)
                return;

            target.TakeDamage(damageData);
        }
    }

}
