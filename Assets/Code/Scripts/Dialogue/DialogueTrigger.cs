using Cinemachine;
using UnityEngine;
using Zenject;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _camera;
	public TextAsset inkAsset;

	[Inject]
	private DialogueService _dialogueService;

	private void Awake()
	{
		_dialogueService.CloseDialogue += OnCloseDialog;
	}

	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue()
	{
		_camera.SetActive(true);
		_dialogueService.Start(inkAsset);
	}

	private void OnCloseDialog()
	{
		_camera.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			TriggerDialogue();
	}

	private void OnDestroy()
	{
		_dialogueService.CloseDialogue -= OnCloseDialog;
	}
}
