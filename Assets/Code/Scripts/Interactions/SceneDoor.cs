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

    public void EndInteraction()
    {
        return;
    }

    public void FocusInteraction(bool isFocused)
    {
        return;
    }

    public string GetActionName()
    {
        return "Przejdź";
    }

    public string GetObjectName()
    {
        return visibleName;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        levelService.ChangeLevel(levelIndex, spawnName);
    }

    public bool InteractionAllowed() => true;

    public bool IsBlocking() => false;

    public bool IsEnabled() => this.enabled;

    public bool ShouldPlayAnimation() => false;
}
