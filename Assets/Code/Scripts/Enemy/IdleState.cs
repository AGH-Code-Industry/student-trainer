using UnityEngine;

public class IdleState : EnemyState
{
    // Are the constructors neccessary in inherited classes?
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        string toPlay = enemy.animSet.idle;
        enemy.PlayAnimation(toPlay);
    }

    public override void Update()
    {
        if (PlayerSeen())
        {
            enemy.ChangeState(new ChasingState(enemy));
        }
    }

    public override void Exit() { }

    bool PlayerSeen()
    {
        Vector3 myPos = enemy.transform.position;
        Vector3 playerPos = enemy.playerMovementService.PlayerPosition;

        float dist = Vector3.Distance(myPos, playerPos);
        Vector3 dirToPlayer = playerPos - myPos;
        float angle = Vector3.Angle(enemy.bodyTransform.forward, dirToPlayer);

        return dist < enemy.settings.detectionRange && angle < enemy.settings.detectionFov;
    }
}
