using Combat.Interfaces;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public abstract class HitEffect
    {
        public abstract void Execute(ICombatant attacker, IDamagable target);
    }

}