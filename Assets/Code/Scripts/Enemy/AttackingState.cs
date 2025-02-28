using System.Collections;
using UnityEngine;

public class AttackingState : EnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public AttackingState(Enemy enemy) : base(enemy)
    {
        enemy.animator.Play("Idle");
    }

    public override void Enter() { }

    public override void Update()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            if (!enemy.InAttackRange())
            {
                enemy.ChangeState(new ChasingState(enemy));
                return;
            }

            enemy.StartCoroutine(enemy.PerformAttack());
          
            lastAttackTime = Time.time;
        }
    }

    public override void Exit() { }




}
