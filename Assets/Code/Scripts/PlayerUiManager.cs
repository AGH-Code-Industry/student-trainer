using UnityEngine;
using Zenject;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private GameObject _hud;

    [Inject] DialogueService _dialogueService;

    private void Awake()
    {
        _dialogueService.StartDialogue += OnStartDialogue;
        _dialogueService.CloseDialogue += OnCloseDialogue;
    }

    private void OnStartDialogue(TextAsset dialogue)
    {
        _hud.SetActive(false);
    }

    private void OnCloseDialogue()
    {
        _hud.SetActive(true);
    }

    private void OnDestroy()
    {
        _dialogueService.StartDialogue -= OnStartDialogue;
    }
}