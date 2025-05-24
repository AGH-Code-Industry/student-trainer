using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string id, name, description;
    public QuestStatus status;

    public QuestStepBase[] steps;
    public QuestStepBase[] startingSteps;

    public QuestRewardBase[] rewards;

    public void Activate()
    {

    }

    public void UpdateActiveSteps()
    {

    }

    public void Complete()
    {

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