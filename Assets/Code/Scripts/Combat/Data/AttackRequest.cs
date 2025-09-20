using System.Collections.Generic;
using Combat.Interfaces;

namespace Combat.Data
{
    public class AttackRequest
    {
        public ICombatant attacker;
        public List<IDamagable> targets;
        public List<HitEffect> hitEffects;
    }
}