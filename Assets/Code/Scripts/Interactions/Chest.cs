using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] string containerName;
    [SerializeField] int containerSize;

    public Container container { get; private set; } = null;
    [Inject] readonly InventoryService service;

    public event System.Action onObjectChanged, onInteractionDestroyed;

    void Awake()
    {
        CreateContainer(containerSize, containerName);
        //Debug.Log($"{gameObject.name}: creating container (\"{containerName}\", size {containerSize})...");
    }

    public void CreateContainer(int size, string name)
    {
        container = new Container(size, name);
    }

    public bool IsEnabled() => this.enabled;

    public void FocusInteraction(bool isFocused)
    {
        if (!isFocused)
            service.ClearContainer();
    }

    public string GetActionName()
    {
        return "Przeglądaj";
    }

    public string GetObjectName()
    {
        return containerName;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        service.SetContainer(container);
    }

    public void EndInteraction()
    {
        service.ClearContainer();
    }

    public bool InteractionAllowed()
    {
        return true;
    }

    public bool IsBlocking()
    {
        return true;
    }

    public bool ShouldPlayAnimation()
    {
        return true;
    }

    void OnDisable()
    {
        onInteractionDestroyed?.Invoke();
    }

    void OnDestroy()
    {
        onInteractionDestroyed?.Invoke();
    }
}
