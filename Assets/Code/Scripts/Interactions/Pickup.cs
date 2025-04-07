using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] ItemPreset item;
    [SerializeField] int count;

    [Inject] readonly InventoryService service;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        service.AddItem(item, count);

        Destroy(gameObject);
    }

    public string GetObjectName()
    {
        string result = item.name;
        if (item.maxStackSize > 1)
            result += $"(x {count})";

        return result;
    }

    public string GetActionName()
    {
        return "Podnieś";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool InteractionAllowed()
    {
        return true;
    }
}
