using UnityEngine;
using Zenject;

public class DialogueTrigger : MonoBehaviour
{

	public TextAsset inkAsset;

	[Inject]
	private DialogueService _dialogueService;

	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue()
	{
		_dialogueService.StartDialogue(inkAsset);
	}
}
