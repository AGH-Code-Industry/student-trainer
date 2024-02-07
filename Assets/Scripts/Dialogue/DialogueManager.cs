using Chapter.Singleton;
using System;
using Ink.Runtime;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager> {

	public bool IsOpenDialog { get; private set; } = false;

	public void Start() 
	{
		UIEvents.CloseDialogue += OnCloseDialogue;
	}

	public void StartDialogue (TextAsset dialogue)
	{
		if(IsOpenDialog) return;

		IsOpenDialog = true;
		InputManager.Instance.GetInput().Disable();
		Events.DialogTrigger.Invoke(dialogue);
	}

	private void OnCloseDialogue()
	{
		IsOpenDialog = false;
		InputManager.Instance.GetInput().Enable();
	}

	private void OnDestroy()
    {
        UIEvents.CloseDialogue -= OnCloseDialogue;
    }

}