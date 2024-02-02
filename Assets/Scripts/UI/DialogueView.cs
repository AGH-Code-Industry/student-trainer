using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Ink.Runtime;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour 
{
    public ScrollRect scrollRect;
    public RectTransform viewContent;
	public GameObject dialogueBox;

	public Animator scrollViewAnimator;
	private Story dialogue;
	private int StoryChunkCount;
	

	void Start () {
		Events.DialogTrigger += OnDisplayDialogue;
		UIEvents.SelectedDialogueChoice += OnSelectDialogueChoice;
	}

    public void OnDisplayDialogue (TextAsset dialogueText)
	{
		dialogue = new Story(dialogueText.text);

		var storyChunk = LoadStoryChunk();
		StoryChunkCount = 1;

		var box = Instantiate(dialogueBox,viewContent.pivot, quaternion.identity,  viewContent.transform);
		box.GetComponent<DialogueBoxView>().SetDialogue(storyChunk);
		box.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, DialogueBoxView.FIRST_ELEMENT_OFFSET);
		
		viewContent.sizeDelta = new Vector2(viewContent.sizeDelta.x, DialogueBoxView.HEIGHT * StoryChunkCount);
		scrollRect.verticalNormalizedPosition = 0;
		scrollViewAnimator.SetBool("IsOpen", true);
	}


	DialogueBoxData LoadStoryChunk()
    {
        string text = "";
		List<string> tags = new List<string>();

        while (dialogue.canContinue)
        {
            text += dialogue.Continue();
			tags.AddRange(dialogue.currentTags);
        }
		string targetName = tags.Find(t => t.Contains("speaker:"))?.Split("speaker:").Last() ?? "";
		string targetImage = tags.Find(t => t.Contains("image:"))?.Split("image:").Last() ?? "";

		string[] currentChoices = dialogue.currentChoices.Select(c => c.text).ToArray();

        return new DialogueBoxData(){targetName = targetName, targetDialogue = text, targetImage = targetImage, yourChoices = currentChoices};
    }

	private void OnSelectDialogueChoice(int choiceIndex)
	{
		if(dialogue.currentChoices.Count == 0 && dialogue.canContinue == false)
		{
			EndDialogue();
			return;
		}

		dialogue.ChooseChoiceIndex(choiceIndex);

		var storyChunk = LoadStoryChunk();
		StoryChunkCount++;
		
		var box = Instantiate(dialogueBox,viewContent.pivot, quaternion.identity,  viewContent.transform);
		box.GetComponent<DialogueBoxView>().SetDialogue(storyChunk);
		box.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, DialogueBoxView.FIRST_ELEMENT_OFFSET - ((StoryChunkCount-1) * DialogueBoxView.HEIGHT));
		
		viewContent.sizeDelta = new Vector2(viewContent.sizeDelta.x, DialogueBoxView.HEIGHT * StoryChunkCount);
		scrollRect.verticalNormalizedPosition = 0;
	}

	void EndDialogue()
	{
		scrollViewAnimator.SetBool("IsOpen", false);
        UIEvents.CloseDialogue.Invoke();

		foreach(Transform child in viewContent.transform)
			Destroy(child.gameObject);
	}

	private void OnDestroy() {
		Events.DialogTrigger -= OnDisplayDialogue;
		UIEvents.SelectedDialogueChoice -= OnSelectDialogueChoice;
	}
}