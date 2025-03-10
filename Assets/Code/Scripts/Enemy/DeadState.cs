using UnityEngine;

public class DeadState : EnemyState
{
    public DeadState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy has dieded");
        enemy.agent.isStopped = true;
        string toPlay = enemy.animSet.death;
        enemy.PlayAnimation(toPlay);
        MonoBehaviour.Destroy(enemy.gameObject, 2f);
    }

    public override void Update() { }

    public override void Exit() { }
}
