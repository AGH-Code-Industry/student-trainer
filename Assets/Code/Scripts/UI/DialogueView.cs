using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Ink.Runtime;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.UI;
using System;

public class DialogueView : MonoBehaviour 
{
    public ScrollRect scrollRect;
    public RectTransform viewContent;
	public GameObject dialogueBox;
	public Animator scrollViewAnimator;
	
	private Story story;
	private int StoryChunkCount;
	private string lastChoice = "";
	private readonly Queue<DialogueBoxData> dialogueBoxes = new();
	

	void Awake () 
	{
		Events.DialogTrigger += OnDisplayDialogue;
		UIEvents.SelectedDialogueChoice += OnSelectDialogueChoice;
		UIEvents.NextDialogue += OnNextDialogue;
		UIEvents.CloseDialogue += OnCloseDialogue;
	}

    public void OnDisplayDialogue (TextAsset dialogueText)
	{
		story = new Story(dialogueText.text);
		
		LoadStoryChunk();
		if(dialogueBoxes.Count == 0)
			return;

		StoryChunkCount = 0;
		CreateDialogueBox(dialogueBoxes.Dequeue());
		
		scrollViewAnimator.SetBool("IsOpen", true);
	}

	void CreateDialogueBox(DialogueBoxData data)
	{
		var box = Instantiate(dialogueBox,viewContent.pivot, quaternion.identity,  viewContent.transform);
		box.GetComponent<DialogueBoxView>().SetDialogue(data);
		box.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -DialogueBoxView.HEIGHT * StoryChunkCount);
		
		viewContent.sizeDelta = new Vector2(viewContent.sizeDelta.x, DialogueBoxView.HEIGHT * (StoryChunkCount + 1));
		scrollRect.verticalNormalizedPosition = 0;

		StoryChunkCount++;
	}

	DialogueBoxData? GetStoryLine()
	{
        if (story.canContinue)
        {
            var line = story.Continue();

			var name = line.Contains(":") ? line.Split(":").First() : "";
			var dialogue = line.Split(":").Last();
			var choices = story.currentChoices.Select(c => c.text).ToArray();
			var image = story.currentTags.Find(t => t.Contains("image:"))?.Split("image:").Last() ?? "";
			var type = name == "hero" ? DialogueType.Hero_Dialogue : DialogueType.Npc;

			return new DialogueBoxData(){type = type, name = name, dialogue = dialogue, image = image, choices = choices };
        }
		return null;
	}

	void LoadStoryChunk()
    {
		DialogueBoxData? dialogueBox;
        while ((dialogueBox = GetStoryLine()) != null)
        {
			var box = dialogueBox.Value;
			if(box.dialogue.Trim().Equals(lastChoice) == false)
				dialogueBoxes.Enqueue(box);

			if(dialogueBox.Value.choices.Length > 0){
				box.type = DialogueType.Hero_Choices;
				dialogueBoxes.Enqueue(box);
			}
        }
		if(story.currentChoices.Count == 0)
			dialogueBoxes.Enqueue(new (){type = DialogueType.End });
    }

	private void OnCloseDialogue() => EndDialogue();

	private void OnNextDialogue() => CreateDialogueBox(dialogueBoxes.Dequeue());

	private void OnSelectDialogueChoice(int choiceIndex)
	{
		if (choiceIndex < story.currentChoices.Count)
		{
			lastChoice = story.currentChoices[choiceIndex].text;
			story.ChooseChoiceIndex(choiceIndex);
		}
		
		LoadStoryChunk();
		OnNextDialogue();
	}

	void EndDialogue()
	{
		scrollViewAnimator.SetBool("IsOpen", false);
		dialogueBoxes.Clear();

		foreach(Transform child in viewContent.transform)
			Destroy(child.gameObject);
	}

	private void OnDestroy() {
		Events.DialogTrigger -= OnDisplayDialogue;
		UIEvents.SelectedDialogueChoice -= OnSelectDialogueChoice;
		UIEvents.NextDialogue -= OnNextDialogue;
		UIEvents.CloseDialogue -= OnCloseDialogue;
	}
}