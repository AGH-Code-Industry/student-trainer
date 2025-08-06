using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Quests
{

    public class Quest
    {
        public string id, name, description;
        public QuestStatus status;

        public QuestStepBase[] steps;
        public List<QuestStepBase> activeSteps;
        //public QuestStepBase[] startingSteps;
        public string[] startingStepIDs;

        public QuestRewardBase[] rewards;

        [Inject] readonly QuestService questService;

        public Quest(string id, string name, string description, QuestStepBase[] steps, string[] startingStepIDs, QuestRewardBase[] rewards)
        {
            this.id = id;
            this.name = name;
            this.description = description;

            this.steps = steps;
            this.startingStepIDs = startingStepIDs;
            activeSteps = new List<QuestStepBase>();

            this.rewards = rewards;
        }

        public void Activate()
        {
            status = QuestStatus.Ongoing;

            foreach(QuestStepBase step in steps)
            {
                step.parentQuest = this;
                foreach(string stepID in startingStepIDs)
                {
                    if (step.id == stepID)
                    {
                        step.StepBegin();
                        break;
                    }
                }
            }
        }

        public void UpdateActiveSteps()
        {
            foreach (QuestStepBase step in activeSteps)
            {
                step.StepUpdate();
            }
        }

        public void Complete(bool failed = false)
        {
            status = failed ? QuestStatus.Failed : QuestStatus.Completed;

            questService.OnQuestCompleted(id);
        }
    }

    public enum QuestStatus
    {
        Waiting,
        Avaiable,
        Ongoing,
        Completed,
        Failed
    }

}