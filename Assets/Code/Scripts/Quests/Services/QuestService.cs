using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestService : IInitializable
{
    public Quest[] _gameQuests;

    public List<Quest> activeQuests;
    public List<Quest> completedQuests;

    [Inject] readonly ResourceReader reader;

    public void Initialize()
    {
        //QuestPreset[] presets = (QuestPreset[])reader.ReadAllSettings<QuestPreset>();

        //hee hee
    }

    public Quest GetQuestByID(string id)
    {
        foreach(Quest quest in _gameQuests)
        {
            if (quest.id == id)
                return quest;
        }

        return null;
    }

    public bool IsQuestActive(string id)
    {
        foreach(Quest quest in activeQuests)
        {
            if (quest.id == id)
                return true;
        }

        return false;
    }

    public void ActivateQuest(string id)
    {
        Quest toActivate = GetQuestByID(id);
        if(toActivate == null)
        {
            Debug.LogError($"QuestService: quest with id \"{id}\" does not exist in the _gameQuests array!");
            return;
        }
        else if(IsQuestActive(id))
        {
            Debug.LogError($"QuestService: tried to activate quest with id \"{id}\", which is already active!");
            return;
        }

        toActivate.Activate();
        activeQuests.Add(toActivate);
    }

    public void CompleteQuest(string id, bool failed = false)
    {

    }
}
