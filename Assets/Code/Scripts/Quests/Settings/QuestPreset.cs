using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Settings/Quest")]
public class QuestPreset : ScriptableObject
{
    public string id;
    public new string name;
    [TextArea]
    public string description;

    public PresetStep[] steps;
    public PresetReward[] rewards;

    [System.Serializable]
    public struct PresetStep
    {

    }

    [System.Serializable]
    public struct PresetReward
    {

    }
}
