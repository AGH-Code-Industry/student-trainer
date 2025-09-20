using Combat.Data;
using Combat.Interfaces;

namespace Combat.Services
{
    public class CombatService
    {
        public void ResolveAttack(AttackRequest data)
        {
            foreach (IDamagable target in data.targets)
            {
                data.hitEffects.ForEach(effect => effect.Execute(data.attacker, target));
            }
        }
    }
}