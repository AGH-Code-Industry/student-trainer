using UnityEngine;

public class ChasingState : EnemyState
{
    public ChasingState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = false;

        string toPlay = enemy.animSet.runForward;
        enemy.PlayAnimation(toPlay);
    }

    public override void Update()
    {
        var playerPos = enemy.playerMovementService.PlayerPosition;
        enemy.agent.SetDestination(playerPos);

        if (InAttackRange())
        {
            enemy.ChangeState(new AttackingState(enemy));
        }
    }

    public override void Exit() { }

    bool InAttackRange()
    {
        // Change later to iterate theough avaiable attack combos,
        // and return true if all attacks in all combos will be able to reach the target

        Vector3 myPos = enemy.transform.position;
        Vector3 playerPos = enemy.playerMovementService.PlayerPosition;

        return Vector3.Distance(myPos, playerPos) <= enemy.attackRange;
    }
}
