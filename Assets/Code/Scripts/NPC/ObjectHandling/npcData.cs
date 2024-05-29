using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NPC", menuName = "NPC")]
public class npcData : ScriptableObject
{
    public new string name;
    public TextAsset dialogue;
    public List<Task> routine;
}
