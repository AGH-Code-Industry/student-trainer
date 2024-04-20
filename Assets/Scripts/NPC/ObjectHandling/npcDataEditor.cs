using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(NPC))]
public class NPCDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (npc.routine != null)
        {
            EditorGUILayout.Space();

            for (int i = 0; i < npc.routine.Count; i++)
            {
                EditorGUILayout.LabelField(npc.routine[i].taskName, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Start Time", npc.routine[i].startTime.GetFormattedTime());
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
    }
}
