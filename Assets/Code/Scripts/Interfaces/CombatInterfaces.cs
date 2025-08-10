namespace Combat
{

    public interface IHitResult
    {
        Combat.AttackResult GetAttackResult(DamageInfo damage);
    }

    public interface IBlockingHandler
    {
        void HandleBlock();
    }

    public interface IDodgeHandler
    {
        void HandleDodge();
    }

}