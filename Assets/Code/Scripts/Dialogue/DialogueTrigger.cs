using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour {

	public TextAsset inkAsset;

	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue ()
	{
		DialogueManager.Instance.StartDialogue(inkAsset);
	}
}
