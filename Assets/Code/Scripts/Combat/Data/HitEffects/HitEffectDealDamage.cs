using UnityEngine;

namespace Combat
{

    public class HitEffectDealDamage : HitEffect
    {
        public DamageData damageData;

        public override void Execute(IDamageable target)
        {
            if (target == null)
                return;

            target.TakeDamage(damageData);
        }
    }

}
