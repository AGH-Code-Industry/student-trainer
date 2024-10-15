using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Ink.Runtime;
using System.Linq;

public class DialogueService
{
    public event Action<TextAsset> StartDialogue;
    public event Action CloseDialogue;

    public bool IsOpenDialog { get; private set; } = false;

    private Queue<DialogueBoxData> _dialogueBoxes;
    private Story _story;
    private string _lastChoice = "";

    [Inject] private readonly InputService _input;
    [Inject] private readonly CameraService _cameraService;

    public void Start(TextAsset dialogue)
    {
        if (IsOpenDialog) return;

        _story = new Story(dialogue.text);
        _dialogueBoxes = new Queue<DialogueBoxData>();

        IsOpenDialog = true;
        _input.Disable();
        StartDialogue.Invoke(dialogue);
    }

    public void Close()
    {
        _dialogueBoxes.Clear();

        IsOpenDialog = false;
        _input.Enable();
        CloseDialogue.Invoke();

        _cameraService.SetActiveCamera(VirtualCameraType.Player);
    }

    public DialogueBoxData GetDialogueBox()
    {
        var box = _dialogueBoxes.Dequeue();
        _cameraService.SetDialogueCamera(box.type);
        return box;
    }

    public int GetDialogueBoxCount() => _dialogueBoxes.Count;

    public void SetLastChoice(string choice) => _lastChoice = choice;

    public void LoadStoryChunk()
    {
        DialogueBoxData? dialogueBox;
        while ((dialogueBox = GetStoryLine()) != null)
        {
            var box = dialogueBox.Value;
            if (box.dialogue.Trim().Equals(_lastChoice) == false)
                _dialogueBoxes.Enqueue(box);

            if (dialogueBox.Value.choices.Length > 0)
            {
                box.type = DialogueType.Hero_Choices;
                _dialogueBoxes.Enqueue(box);
            }
        }
        if (_story.currentChoices.Count == 0)
            _dialogueBoxes.Enqueue(new() { type = DialogueType.End });
    }

    public void MakeChoice(int choiceIndex)
    {
        if (choiceIndex < _story.currentChoices.Count)
        {
            _lastChoice = _story.currentChoices[choiceIndex].text;
            _story.ChooseChoiceIndex(choiceIndex);
        }
    }

    private DialogueBoxData? GetStoryLine()
    {
        if (_story.canContinue)
        {
            var line = _story.Continue();

            var name = line.Contains(":") ? line.Split(":").First() : "";
            var dialogue = line.Split(":").Last();
            var choices = _story.currentChoices.Select(c => c.text).ToArray();
            var image = _story.currentTags.Find(t => t.Contains("image:"))?.Split("image:").Last() ?? "";
            var type = name == "Hero" ? DialogueType.Hero_Dialogue : DialogueType.Npc;

            return new DialogueBoxData() { type = type, name = name, dialogue = dialogue, image = image, choices = choices };
        }
        return null;
    }


}