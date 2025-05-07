using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] string containerName;
    [SerializeField] int containerSize;

    Container container;
    [Inject] readonly InventoryService service;

    public event System.Action onObjectChanged;

    void Start()
    {
        container = new Container(containerSize, containerName);
    }

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
}
