using UnityEngine;
using Zenject;

namespace Quests
{

    public class KillEnemiesStep : QuestStepBase
    {
        EnemiesToKill requiredKills;

        public KillEnemiesStep(string id, string stepText, QuestStepBase[] req, QuestStepBase[] next, StepResult compRes, StepResult failRes, string enemyID, int amount, string enemyName) : base(id, stepText, req, next, compRes, failRes)
        {
            requiredKills = new EnemiesToKill(enemyID, enemyName, amount);
        }

        public override void StepBegin()
        {
            base.StepBegin();

            eventBus.Subscribe<EnemyKilledEvent>(EnemyKilled);
        }

        public override void StepUpdate()
        {
            return;
        }

        public override void StepEnd()
        {
            eventBus.Unsubscribe<EnemyKilledEvent>(EnemyKilled);
            status = QuestStepStatus.Completed;
            base.StepEnd();
        }

        public override string GetStepText()
        {
            return $"{stepText} ({requiredKills.currentAmount} / {requiredKills.requiredAmount}) {requiredKills.enemyName}";
        }

        void EnemyKilled(EnemyKilledEvent ev)
        {
            if(ev.enemyID == requiredKills.enemyID)
            {
                requiredKills.currentAmount++;
                if (requiredKills.IsComplete())
                    StepEnd();
            }
        }

        struct EnemiesToKill
        {
            public string enemyID, enemyName;
            public int requiredAmount, currentAmount;

            public EnemiesToKill(string enemyID, string enemyName, int requiredAmount)
            {
                this.enemyID = enemyID;
                this.enemyName = enemyName;
                this.requiredAmount = requiredAmount;
                this.currentAmount = 0;
            }

            public bool IsComplete() => currentAmount >= requiredAmount;
        }
    }

}
