using Combat.Interfaces;
using UnityEngine;

namespace Combat
{

    public class HitEffectDealDamage : HitEffect
    {
        public DamageData damageData;

        public override void Execute(IDamagable target)
        {
            if (target == null)
                return;

            target.TakeDamage(damageData);
        }
    }

}
