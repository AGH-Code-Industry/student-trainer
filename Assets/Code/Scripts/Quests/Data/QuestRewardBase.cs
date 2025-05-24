using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestRewardBase
{
    public string rewardText;
    public bool isVisible;

    public abstract void GiveReward();
}
