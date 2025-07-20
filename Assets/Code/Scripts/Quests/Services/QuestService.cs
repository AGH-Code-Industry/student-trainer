using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Quests
{

    public class QuestService : MonoBehaviour, IInitializable
    {
        public Quest[] _gameQuests;

        public List<Quest> activeQuests;
        public List<Quest> completedQuests;

        [Inject] readonly ResourceReader reader;
        [Inject] readonly EventBus eventBus;
        [Inject] readonly DiContainer Container;

        public void Initialize()
        {
            activeQuests = new List<Quest>();
            completedQuests = new List<Quest>();

            //QuestPreset[] presets = (QuestPreset[])reader.ReadAllSettings<QuestPreset>();

            // Create the demo quest manually here

            HaveItemsStep itemStep = new HaveItemsStep("demo_item", "Zdobądź", null, null, StepResult.CompleteQuest, StepResult.Nothing, "key1", 1);
            Container.Inject(itemStep);

            QuestStepBase[] nextKill = { itemStep };

            KillEnemiesStep killStep = new KillEnemiesStep("demo_kill", "Pokonaj", null, nextKill, StepResult.Nothing, StepResult.Nothing, "student_debil", 1, "studenta");
            Container.Inject(killStep);

            QuestStepBase[] nextArea = { killStep };
            
            ReachAreaStep areaStep = new ReachAreaStep("demo_reach_area", "Dotrzyj do oznaczonego (czerownego) obszaru", null, nextArea, StepResult.Nothing, StepResult.Nothing, "test_area");
            // Need to manually inject dependancies into classes created at runtime
            Container.Inject(areaStep);

            List<QuestStepBase> steps = new List<QuestStepBase>();
            steps.Add(areaStep);
            steps.Add(killStep);
            steps.Add(itemStep);

            List<string> startingSteps = new List<string>();
            startingSteps.Add("demo_reach_area");

            Quest demoQuest = new Quest("demo_quest", "Testowy Quest", "description", steps.ToArray(), startingSteps.ToArray(), null);
            Container.Inject(demoQuest);
            _gameQuests = new Quest[1];
            _gameQuests[0] = demoQuest;
        }

        void Update()
        {
            // Update quests each frame, maybe change to FixedUpdate
            foreach (Quest quest in activeQuests)
            {
                quest.UpdateActiveSteps();
            }
        }

        Quest CreateQuestFromPreset(QuestPreset _preset)
        {
            return null;
        }

        public Quest GetQuestByID(string id)
        {
            foreach (Quest quest in _gameQuests)
            {
                if (quest.id == id)
                    return quest;
            }

            return null;
        }

        public bool IsQuestActive(string id)
        {
            foreach (Quest quest in activeQuests)
            {
                if (quest.id == id)
                    return true;
            }

            return false;
        }

        public void ActivateQuest(string id)
        {
            Quest toActivate = GetQuestByID(id);
            if (toActivate == null)
            {
                Debug.LogError($"QuestService, ActivateQuest(): quest with id \"{id}\" does not exist in the _gameQuests array!");
                return;
            }
            else if (IsQuestActive(id))
            {
                Debug.LogError($"QuestService, ActivateQuest(): tried to activate quest with id \"{id}\", which is already active!");
                return;
            }
            else if(completedQuests.Contains(toActivate))
            {
                Debug.Log($"QuestService: quest \"{id}\" tried to activate, but it was already completed.");
                return;
            }

            toActivate.Activate();
            eventBus.Publish(new QuestStatusChanged(id, toActivate.status));
            activeQuests.Add(toActivate);

            Debug.Log($"QUEST \"{id}\" started");
        }

        public void OnQuestCompleted(string id)
        {
            Quest quest = GetQuestByID(id);
            if (quest == null)
            {
                Debug.LogError($"QuestService, OnQuestCompleted(): quest with id \"{id}\" does not exist in the _gameQuests array!");
                return;
            }
            else if (!IsQuestActive(id))
            {
                Debug.LogError($"QuestService, OnQuestCompleted(): tried to complete quest with id \"{id}\", which isn't active!");
                return;
            }

            eventBus.Publish(new QuestStatusChanged(id, quest.status));
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            Debug.Log($"QUEST \"{id}\" completed.");
        }
    }

}