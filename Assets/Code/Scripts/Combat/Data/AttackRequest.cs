using System.Collections.Generic;
using Combat.Interfaces;

namespace Combat.Data
{
    public struct AttackRequest
    {
        public ICombatant attacker;
        public List<IDamagable> targets;
        public List<HitEffect> hitEffects;
    }
}