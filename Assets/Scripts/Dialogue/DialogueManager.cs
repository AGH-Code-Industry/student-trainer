using Chapter.Singleton;
using System;
using Ink.Runtime;

public class DialogueManager : Singleton<DialogueManager> {

	public bool IsOpenDialog { get; private set; } = false;

	public void Start() 
	{
		UIEvents.CloseDialogue += OnCloseDialogue;
	}

	public void StartDialogue (Story dialogue)
	{
		if(IsOpenDialog) return;

		IsOpenDialog = true;
		Events.DialogTrigger.Invoke(dialogue);
		//Game.Pause();
	}

	private void OnCloseDialogue()
	{
		IsOpenDialog = false;
	}

	private void OnDestroy()
    {
        UIEvents.CloseDialogue -= OnCloseDialogue;
    }

}