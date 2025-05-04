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

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            service.ClearContainer();
    }

    public void FocusInteraction(bool isFocused)
    {
        // nothing...
    }

    public string GetActionName()
    {
        return "Przeglądaj";
    }

    public string GetObjectName()
    {
        return "Skrzynia";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        service.SetContainer(container);
    }

    public bool InteractionAllowed()
    {
        return true;
    }
}
