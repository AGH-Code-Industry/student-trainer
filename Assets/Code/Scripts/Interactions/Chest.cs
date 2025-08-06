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
    }

    void Start()
    {
        //FindAnyObjectByType<PlayerInteractions>().RegisterInteractable(this);
    }

    public void CreateContainer(int size, string name)
    {
        if(container == null)
            container = new Container(size, name);
    }



    public void Interact()
    {
        service.SetContainer(container);
        FindAnyObjectByType<UiManager>().OpenWindow("ChestUI");
    }

    public GameObject GetGO() => gameObject;

    public InteractableData GetInteractionData()
    {
        return new InteractableData(containerName, "przeglądaj", true, true);
    }

    public void FocusInteraction(bool isFocused)
    {
        if (!isFocused)
            service.ClearContainer();
    }

    void OnEnable()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RegisterInteractable(this);
    }

    void OnDisable()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
        onInteractionDestroyed?.Invoke();

        FindAnyObjectByType<UiManager>()?.CloseWindow("ChestUI");
    }

    /*
    void OnDestroy()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
        onInteractionDestroyed?.Invoke();
    }
    */
}
