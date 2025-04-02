using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pickup : MonoBehaviour, IInteractable
{
    //public ItemPreset item { get; private set; } = null;
    //public int count { get; private set; } = 0;

    [SerializeField] ItemPreset item;
    [SerializeField] int count;

    [Inject] readonly InventoryService service;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void AssignItem(ItemPreset item, int count)
    {
        this.item = item;
        this.count = count;
    }
    */

    public void Interact()
    {
        service.AddItem(item, count);

        Destroy(gameObject);
    }
}
