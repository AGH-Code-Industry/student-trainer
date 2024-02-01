using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour {

	// Set this file to your compiled json asset
	public TextAsset inkAsset;

	// The ink story that we're wrapping
	Story _inkStory;

	private void Start() {
		_inkStory = new Story(inkAsset.text);

	}

	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue ()
	{
		DialogueManager.Instance.StartDialogue(_inkStory);
		_inkStory.ResetState();
	}
}
