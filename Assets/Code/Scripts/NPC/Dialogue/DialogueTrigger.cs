using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour
{

	private TextAsset inkAsset;
	private NPC npc;
	void Start()
	{
		npc = GetComponent<NPC>();
		inkAsset = npc.data.dialogue;
	}
	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue()
	{
		//DialogueManager.Instance.StartDialogue(inkAsset);
	}
}
