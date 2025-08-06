using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneDoor : MonoBehaviour, IInteractable
{
    [SerializeField] string levelIndex, visibleName, spawnName;

    public event Action onObjectChanged;
    public event Action onInteractionDestroyed;

    [Inject] readonly LevelService levelService;

    void Start()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RegisterInteractable(this);
    }

    public void Interact()
    {
        levelService.ChangeLevel(levelIndex, spawnName);
    }

    public GameObject GetGO() => gameObject;

    public InteractableData GetInteractionData()
    {
        return new InteractableData(visibleName, "przejdź", false, true);
    }

    public void FocusInteraction(bool isFocused)
    {
        return;
    }

    private void OnDestroy()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
    }
}
