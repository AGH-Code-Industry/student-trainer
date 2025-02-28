public class DeadState : EnemyState
{
    public DeadState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.Die();
    }

    public override void Update() { }

    public override void Exit() { }
}
