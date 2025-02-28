public class ChasingState : EnemyState
{
    public ChasingState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.animator.Play("Run");
    }

    public override void Update()
    {
        var playerPos = enemy.playerMovementService.PlayerPosition;
        enemy.agent.SetDestination(playerPos - playerPos.normalized);

        if (enemy.InAttackRange())
        {
            enemy.ChangeState(new AttackingState(enemy));
        }
    }

    public override void Exit() { }
}
