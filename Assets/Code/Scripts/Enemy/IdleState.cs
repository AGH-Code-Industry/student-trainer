public class IdleState : EnemyState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.animator.Play("Idle");
    }

    public override void Update()
    {
        if (enemy.PlayerInRange())
        {
            enemy.ChangeState(new ChasingState(enemy));
        }
    }

    public override void Exit() { }
}
