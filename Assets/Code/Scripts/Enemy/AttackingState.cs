using System.Collections;
using UnityEngine;

public class AttackingState : EnemyState
{
    const float ATTACK_RANGE = 1.7f;

    private ComboSystem comboSystem;

    public AttackingState(Enemy enemy) : base(enemy) { }

    bool attackInProgress = false;
    bool comboSwitchRequired = false;

    public override void Enter()
    {
        enemy.PlayAnimation(enemy.animSet.idle);

        ComboList comboList = enemy.settings.attacks;
        // Select the first combo, doesn't really matter
        string defaultCombo = comboList.combos[0].name;
        MonoBehaviour mediator = enemy;

        comboSystem = new ComboSystem(comboList, defaultCombo, mediator);
        comboSystem.onAttackStart += AttackStarted;
        comboSystem.onAttackPerformed += AttackPerformed;
        comboSystem.onAttackEnd += AttackEnded;
        comboSystem.onRecoveryEnd += RecoveryEnded;

        SwitchCombo();
    }

    public override void Update()
    {
        if (!InAttackRange() || !attackInProgress)
        {
            enemy.ChangeState(new ChasingState(enemy));
        }
        else if(InAttackRange())
        {
            if(!attackInProgress)
                SwitchCombo();
        }
    }

    public override void Exit() { }

    bool InAttackRange()
    {
        // Change later to iterate theough avaiable attack combos,
        // and return true if all attacks in all combos will be able to reach the target

        Vector3 myPos = enemy.transform.position;
        Vector3 playerPos = enemy.playerMovementService.PlayerPosition;

        return Vector3.Distance(myPos, playerPos) < ATTACK_RANGE;
    }

    string SelectRandomCombo()
    {
        // I fonund this code for weighted randomness in a random yt video, so I'm not sure if it actually works as intended

        ComboList combos = enemy.settings.attacks;

        float totalWeight = 0f;
        foreach(ComboWeight w in combos.weights)
        {
            totalWeight += w.weight;
        }

        float randomWeight = Random.Range(0, totalWeight);

        string selectedCombo = "";
        float cumulativeWeight = 0f;
        foreach(ComboWeight w in combos.weights)
        {
            cumulativeWeight += w.weight;
            if(cumulativeWeight >= randomWeight)
            {
                selectedCombo = w.comboName;
                break;
            }
        }

        // Failsafe in case of a typo
        if (!comboSystem.HasCombo(selectedCombo))
            selectedCombo = combos.combos[0].name;

        return selectedCombo;
    }

    void SwitchCombo()
    {
        string combo = SelectRandomCombo();
        comboSystem.ChangeCombo(combo);
        comboSystem.Attack();
    }

    #region combo_system_callbacks

    void AttackStarted(string animName)
    {
        enemy.PlayAnimation(animName);
        attackInProgress = true;
    }

    void AttackPerformed(float damage, float range)
    {
        enemy.Attack(damage, range);
    }

    void AttackEnded(bool isComboEnd)
    {
        if(!isComboEnd)
        {
            if (InAttackRange())
                comboSystem.Attack();
            else
                comboSwitchRequired = true;
        }
    }

    void RecoveryEnded()
    {
        attackInProgress = false;
        if(!InAttackRange())
        {
            enemy.ChangeState(new ChasingState(enemy));
            return;
        }

        if(comboSwitchRequired)
        {
            comboSwitchRequired = false;
            SwitchCombo();
        }
    }

    #endregion
}
