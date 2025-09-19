
namespace Combat.Interfaces
{
    public interface ICombatant : IDamagable, IEffect
    {
        void Attack();
    }
}
