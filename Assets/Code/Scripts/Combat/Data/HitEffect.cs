using Combat.Interfaces;
using UnityEngine;

namespace Combat
{

    public abstract class HitEffect
    {
        // It will take an ICombatant (source) after it's added
        public abstract void Execute(IDamagable target);
    }

}