using System;
using UnityEngine;
using Zenject;

public class DialogueService
{
    public event Action<TextAsset> DialogTrigger;

    public bool IsOpenDialog { get; private set; } = false;

    [Inject]
    private readonly InputService input;

    public void StartDialogue(TextAsset dialogue)
    {
        if (IsOpenDialog) return;

        IsOpenDialog = true;
        input.Disable();
        DialogTrigger.Invoke(dialogue);
    }

    private void OnCloseDialogue()
    {
        IsOpenDialog = false;
        input.Enable();
    }

}