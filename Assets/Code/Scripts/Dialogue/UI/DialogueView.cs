using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.UI;
using Zenject;

public class DialogueView : MonoBehaviour
{
	public ScrollRect scrollRect;
	public RectTransform viewContent;
	public GameObject dialogueBox;
	public Animator scrollViewAnimator;

	private int StoryChunkCount;

	[Inject]
	private readonly DialogueService _dialogueService;

	void Awake()
	{
		_dialogueService.StartDialogue += OnDisplayDialogue;
		DialogueBoxView.SelectedChoice += OnSelectDialogueChoice;
		DialogueBoxView.NextDialogue += OnNextDialogue;
		DialogueBoxView.Close += OnCloseDialogue;
	}

	public void OnDisplayDialogue(TextAsset dialogueText)
	{
		_dialogueService.LoadStoryChunk();

		if (_dialogueService.GetDialogueBoxCount() == 0)
			return;

		StoryChunkCount = 0;
		CreateDialogueBox(_dialogueService.GetDialogueBox());


		scrollViewAnimator.SetBool("IsOpen", true);
	}

	void CreateDialogueBox(DialogueBoxData data)
	{
		var box = Instantiate(dialogueBox, viewContent.pivot, quaternion.identity, viewContent.transform);

		box.GetComponent<DialogueBoxView>().SetDialogue(data);
		box.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -DialogueBoxView.HEIGHT * StoryChunkCount);

		viewContent.sizeDelta = new Vector2(viewContent.sizeDelta.x, DialogueBoxView.HEIGHT * (StoryChunkCount + 1));
		scrollRect.verticalNormalizedPosition = 0;

		StoryChunkCount++;
	}

	private void OnCloseDialogue() => EndDialogue();

	private void OnNextDialogue() => CreateDialogueBox(_dialogueService.GetDialogueBox());

	private void OnSelectDialogueChoice(int choiceIndex)
	{
		_dialogueService.MakeChoice(choiceIndex);
		_dialogueService.LoadStoryChunk();
		OnNextDialogue();
	}

	void EndDialogue()
	{
		scrollViewAnimator.SetBool("IsOpen", false);

		foreach (Transform child in viewContent.transform)
			Destroy(child.gameObject);

		_dialogueService.Close();
	}

	private void OnDestroy()
	{
		_dialogueService.StartDialogue -= OnDisplayDialogue;
		DialogueBoxView.SelectedChoice -= OnSelectDialogueChoice;
		DialogueBoxView.NextDialogue -= OnNextDialogue;
		DialogueBoxView.Close -= OnCloseDialogue;
	}
}