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

            /*
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
            */

            TalkStep finish = new TalkStep("finish", "Porozmawiaj z żulisiem", null, null, StepResult.CompleteQuest, StepResult.Nothing);
            Container.Inject(finish);

            QuestStepBase[] beer_next = { finish };

            HaveItemsStep beerStep = new HaveItemsStep("step_beer", "Weź", null, beer_next, StepResult.Nothing, StepResult.Nothing, "piwo", 1);
            Container.Inject(beerStep);

            QuestStepBase[] room_next = { beerStep };

            ReachAreaStep roomStep = new ReachAreaStep("room_step", "Wejdź do pokoju na pierwszym piętrze", null, room_next, StepResult.Nothing, StepResult.Nothing, "room_area");
            Container.Inject(roomStep);

            QuestStepBase[] key_next = { roomStep };

            HaveItemsStep keyStep = new HaveItemsStep("step_key", "Zdobądź", null, key_next, StepResult.Nothing, StepResult.Nothing, "key1", 1);
            Container.Inject(keyStep);

            KillEnemiesStep killStep = new KillEnemiesStep("step_kill", "Pokonaj", null, null, StepResult.Nothing, StepResult.Nothing, "student_debil", 6, "studentów");
            Container.Inject(killStep);

            QuestStepBase[] area1_next = { killStep, keyStep };

            ReachAreaStep area1 = new ReachAreaStep("reach_lobby", "Dostań się do akademika", null, area1_next, StepResult.Nothing, StepResult.Nothing, "lvl1_area");
            Container.Inject(area1);

            QuestStepBase[] heal_next = { area1 };

            HaveItemsStep healStep = new HaveItemsStep("heal_step", "(Przedmiot leczący) Weź ze śmietnika", null, heal_next, StepResult.Nothing, StepResult.Nothing, "healing_pills", 4);
            Container.Inject(healStep);

            List<QuestStepBase> steps = new List<QuestStepBase>();
            steps.Add(healStep);
            steps.Add(area1);
            steps.Add(killStep);
            steps.Add(keyStep);
            steps.Add(roomStep);
            steps.Add(beerStep);
            steps.Add(finish);

            List<string> startingSteps = new List<string>();
            startingSteps.Add("heal_step");

            Quest demoQuest = new Quest("demo_quest", "Moja druga połówka", "description", steps.ToArray(), startingSteps.ToArray(), null);
            Container.Inject(demoQuest);

// ------------------ na potrzeby demo
            TalkStep first = new TalkStep("first", "Porozmawiaj z Żulisiem", null, null, StepResult.Nothing, StepResult.Nothing);
            Container.Inject(first);

            List<QuestStepBase> steps2 = new List<QuestStepBase>();
            steps2.Add(first);

            List<string> startingSteps2 = new List<string>();
            startingSteps2.Add("talk_step");

            Quest firstQuest = new Quest("firstQuest", "Porozmawiaj z Żulisiem", "description", steps2.ToArray(), startingSteps2.ToArray(), null);
            Container.Inject(firstQuest);
// ------------------

            _gameQuests = new Quest[2];
            _gameQuests[0] = firstQuest;
            _gameQuests[1] = demoQuest;
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

        public bool IsQuestCompleted(string id)
        {
            foreach(Quest quest in completedQuests)
            {
                if(quest.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public QuestStepStatus? GetStepStatus(string questID, string stepID)
        {
            Quest quest = GetQuestByID(questID);

            foreach(QuestStepBase step in quest.steps)
            {
                if (step.id == stepID)
                    return step.status;
            }

            return null;
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