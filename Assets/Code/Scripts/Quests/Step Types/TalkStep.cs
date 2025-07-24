using UnityEngine;
using Zenject;

namespace Quests
{

    public class TalkStep : QuestStepBase
    {
        public TalkStep(string id, string stepText, QuestStepBase[] req, QuestStepBase[] next, StepResult compRes, StepResult failRes) : base(id, stepText, req, next, compRes, failRes)
        {
            
        }

        public override void StepBegin()
        {
            base.StepBegin();

            eventBus.Subscribe<DialogueEndedEvent>(OnDialogueEnd);
        }

        public override void StepUpdate()
        {
            return;
        }

        public override void StepEnd()
        {
            eventBus.Unsubscribe<DialogueEndedEvent>(OnDialogueEnd);
            this.status = QuestStepStatus.Completed;
            base.StepEnd();
        }

        void OnDialogueEnd(DialogueEndedEvent ev)
        {
            StepEnd();
        }
    }

}
