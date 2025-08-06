using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Quests
{

    public abstract class QuestStepBase
    {
        public Quest parentQuest;

        public string id { get; protected set; }
        public string stepText { get; protected set; }
        public QuestStepStatus status { get; protected set; }

        // ignore the required steps for now
        public QuestStepBase[] requiredSteps { get; protected set; }
        public QuestStepBase[] nextSteps { get; protected set; }

        public StepResult completionResult { get; protected set; }
        public StepResult failResult { get; protected set; }

        [Inject] protected readonly EventBus eventBus;

        public QuestStepBase(string id, string stepText, QuestStepBase[] req, QuestStepBase[] next, StepResult compRes = StepResult.Nothing, StepResult failRes = StepResult.Nothing)
        {
            this.id = id;
            this.stepText = stepText;

            status = QuestStepStatus.Waiting;

            requiredSteps = req;
            nextSteps = next;

            completionResult = compRes;
            failResult = failRes;
        }

        public virtual void StepBegin()
        {
            status = QuestStepStatus.Ongoing;
            parentQuest.activeSteps.Add(this);
            Debug.Log($"STEP \"{id}\" started");

            eventBus.Publish(new StepStatusChanged(parentQuest.id, id, status));
        }

        public abstract void StepUpdate();

        public virtual void StepEnd()
        {
            parentQuest.activeSteps.Remove(this);
            Debug.Log($"STEP \"{id}\" finished!");

            eventBus.Publish(new StepStatusChanged(parentQuest.id, id, status));

            if(status == QuestStepStatus.Completed)
            {
                if (completionResult == StepResult.CompleteQuest)
                    parentQuest.Complete(false);
                else if(completionResult == StepResult.FailQuest)
                    parentQuest.Complete(true);
            }
            else if(status == QuestStepStatus.Failed)
            {
                if (completionResult == StepResult.CompleteQuest)
                    parentQuest.Complete(false);
                else if (completionResult == StepResult.FailQuest)
                    parentQuest.Complete(true);
            }

            if (nextSteps != null)
            {
                foreach (QuestStepBase step in nextSteps)
                {
                    step.StepBegin();
                }
            }
        }

        public virtual string GetStepText()
        {
            return stepText;
        }
    }

    public enum QuestStepStatus
    {
        Waiting,
        Ongoing,
        Completed,
        Failed
    }

    public enum StepResult
    {
        Nothing,
        CompleteQuest,
        FailQuest
    }

}