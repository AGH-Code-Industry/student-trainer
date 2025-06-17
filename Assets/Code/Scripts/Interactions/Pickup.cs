using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] ItemPreset item;
    [SerializeField] int count;

    [Inject] readonly InventoryService service;

    public event System.Action onObjectChanged, onInteractionDestroyed;

    void Start()
    {
        FindAnyObjectByType<PlayerInteractions>().RegisterInteractable(this);
    }

    void Update()
    {
        
    }

    

    public void Interact()
    {
        service.AddItem(item, count);

        Destroy(gameObject);
    }

    public GameObject GetGO() => gameObject;

    public InteractableData GetInteractionData()
    {
        // Check if player's inventory is full, and change data accordingly
        bool canPickUp = true;
        string actionMessage = "podnieś";
        return new InteractableData(item.name, actionMessage, false, canPickUp);
    }

    public void FocusInteraction(bool isFocused) { }

    private void OnDestroy()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
    }
}
