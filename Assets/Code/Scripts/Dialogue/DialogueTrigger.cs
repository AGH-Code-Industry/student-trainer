using Cinemachine;
using System;
using UnityEngine;
using Zenject;
using Quests;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
	// This script was hardcoded for the gamefest demo, remove all the hardcoding later

	//kk

	[SerializeField] private GameObject _camera;
	[SerializeField] TextAsset dialogue1, dialogue2;
	[SerializeField] string npcName;

	[Inject] readonly DialogueService _dialogueService;
	[Inject] readonly QuestService questService;
	[Inject] readonly EventBus eventBus;

    public event Action onObjectChanged;
    public event Action onInteractionDestroyed;

    private void Awake()
	{
		_dialogueService.CloseDialogue += OnCloseDialog;
	}

	[ContextMenu("TriggerDialogue")]
	public void TriggerDialogue()
	{
		Debug.Log("Triggering dialogue");
		_camera.SetActive(true);

		if (questService.GetStepStatus("demo_quest", "step_beer") == QuestStepStatus.Completed)
			_dialogueService.Start(dialogue2);
		else
			_dialogueService.Start(dialogue1);
	}

	private void OnCloseDialog()
	{
		_camera.SetActive(false);

		if (!questService.IsQuestActive("demo_quest"))
        {
			questService.ActivateQuest("demo_quest");
        }

		eventBus.Publish(new DialogueEndedEvent());
	}

	void OnEnable()
	{
		FindAnyObjectByType<PlayerInteractions>()?.RegisterInteractable(this);
	}

	void OnDisable()
	{
		FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
		onInteractionDestroyed?.Invoke();
	}

	private void OnDestroy()
	{
		_dialogueService.CloseDialogue -= OnCloseDialog;
	}

    public void Interact()
    {
		TriggerDialogue();
    }

    public GameObject GetGO()
    {
		return gameObject;
    }

    public InteractableData GetInteractionData()
    {
		bool canInteract = !questService.IsQuestActive("demo_quest") || questService.GetStepStatus("demo_quest", "step_beer") == QuestStepStatus.Completed;
		string interName = canInteract ? "rozmawiaj" : "zajęty";
		return new InteractableData(npcName, interName, false, canInteract);
    }

    public void FocusInteraction(bool isFocused)
    {
		return;
    }
}
