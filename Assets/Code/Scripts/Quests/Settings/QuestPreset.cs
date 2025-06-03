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
}
