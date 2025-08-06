using UnityEngine;
using Zenject;

namespace Quests
{

    public class ReachAreaStep : QuestStepBase
    {
        string areaID;

        public ReachAreaStep(string id, string stepText, QuestStepBase[] req, QuestStepBase[] next, StepResult compRes, StepResult failRes, string areaID) : base(id, stepText, req, next, compRes, failRes)
        {
            this.areaID = areaID;
        }

        public override void StepBegin()
        {
            base.StepBegin();

            eventBus.Subscribe<AreaEnteredEvent>(PlayerEnteredArea);
        }

        public override void StepUpdate()
        {
            return;
        }

        public override void StepEnd()
        {
            eventBus.Unsubscribe<AreaEnteredEvent>(PlayerEnteredArea);
            this.status = QuestStepStatus.Completed;
            base.StepEnd();
        }

        void PlayerEnteredArea(AreaEnteredEvent ev)
        {
            if (ev.areaID == areaID)
            {
                StepEnd();
                return;
            }
        }
    }

}
