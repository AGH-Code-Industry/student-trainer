using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Ink.Runtime;
using System.Linq;

public class DialogueView : MonoBehaviour 
{
    public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;

	public Animator animator;

	private Queue<string> sentences;
	private Story dialogue;

	void Start () {
		Events.DialogTrigger += OnDisplayDialogue;

		sentences = new Queue<string>();
	}

    public void OnDisplayDialogue (Story dialogue)
	{
		this.dialogue = dialogue;
		this.dialogue.ResetState();

		animator.SetBool("IsOpen", true);
		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		StopAllCoroutines();
		if (dialogue.canContinue) {
			string story = LoadStoryChunk(dialogue);
			nameText.text = dialogue.currentTags.FirstOrDefault();
			StartCoroutine(TypeSentence(story));
			return;
		}

		EndDialogue();
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	string LoadStoryChunk(Story dialogue)
    {
        string text = "";

        if (dialogue.canContinue)
        {
            text += dialogue.Continue();
        }

        return text;
    }

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
        UIEvents.CloseDialogue.Invoke();
	}

	private void OnDestroy() {
		Events.DialogTrigger -= OnDisplayDialogue;
	}
}