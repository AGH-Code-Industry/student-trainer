using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class InventoryService : IInitializable
{
    public InventorySettings settings { get; private set; }

    public ItemPreset[] availableItems { get; private set; }
    public Slot[] slots { get; private set; }

    [Inject] readonly ResourceReader reader;

    // Pass slot's index as argument
    //public Action<int> onSlotContentsChanged;
    public Action onContentsChanged;

    public void Initialize()
    {
        settings = reader.ReadSettings<InventorySettings>();

        availableItems = (ItemPreset[])reader.ReadAllSettings<ItemPreset>();
        slots = new Slot[settings.inventorySize];

        for(int i = 0; i < settings.inventorySize; i++)
        {
            slots[i] = new Slot();
        }

        // Error to stop unity from compiling when writing code
        //test = 0;
    }

    public void AddItem(ItemPreset item, int amount)
    {
        bool stackable = item.maxStackSize > 1;
        int amountLeft = amount;

        for(int i = 0; i < slots.Length; i++)
        {
            // amountLeft should never go below zero
            Assert.IsTrue(amountLeft >= 0);

            if(stackable)
            {
                // Add items to slot if it contains the same item
                if(slots[i].item == item)
                {
                    amountLeft = slots[i].IncreaseCount(amountLeft);
                    if (amountLeft == 0)
                        break;
                }
            }

            // Add item to slot if slot is free
            if (!slots[i].item)
            {
                amountLeft = slots[i].AssignItem(item, amountLeft);
                if (amountLeft == 0)
                    break;
            }
        }

        onContentsChanged?.Invoke();
    }

    public void AddItemByID(string id, int amount)
    {
        foreach(ItemPreset item in availableItems)
        {
            if(item.id == id)
            {
                AddItem(item, amount);
                return;
            }
        }

        Debug.LogWarning($"Wrong ID ({id}) passed to function AddItemByID().");
    }
}