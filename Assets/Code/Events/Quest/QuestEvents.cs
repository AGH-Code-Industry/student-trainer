using UnityEngine;

namespace Quests
{

    public class BaseQuestEvent
    {
        public string questID;

        public BaseQuestEvent(string questID)
        {
            this.questID = questID;
        }
    }

    public class QuestStatusChanged : BaseQuestEvent
    {
        public QuestStatus newStatus;

        public QuestStatusChanged(string questID, QuestStatus newStatus) : base(questID)
        {
            this.newStatus = newStatus;
        }
    }

    public class StepStatusChanged : BaseQuestEvent
    {
        public string stepID;
        public QuestStepStatus newStatus;

        public StepStatusChanged(string questID, string stepID, QuestStepStatus newStatus) : base(questID)
        {
            this.stepID = stepID;
            this.newStatus = newStatus;
        }
    }

    public class StepUpdated : BaseQuestEvent
    {
        public string stepID;

        public StepUpdated(string questID, string stepID) : base(questID)
        {
            this.stepID = stepID;
        }
    }

}
