/*

    FEATURES TO ADD:
    - UI toolbar when cursor is over slot, showing item's name (and maybe description)
    - Moving items by dragging them between slots (need to communicate with combat system, so the player won't attack when clicking in the inv)
    - Dropping items on the ground
    - Splitting item stacks in half by pressing the scroll wheel?
    - Hotkeys (using items by pressing numbers 1-9)

*/

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
    [Inject] readonly ItemUsingService itemService;

    // Pass slot's index as argument
    //public Action<int> onSlotContentsChanged;
    public Action onContentsChanged;

    // Types of items that can be used
    readonly Type[] usableTypes = { typeof(HealingItemPreset), typeof(BuffItemPreset) };

    public void Initialize()
    {
        settings = reader.ReadSettings<InventorySettings>();

        availableItems = (ItemPreset[])reader.ReadAllSettings<ItemPreset>();
        slots = new Slot[settings.inventorySize];

        for(int i = 0; i < settings.inventorySize; i++)
        {
            slots[i] = new Slot();
        }
    }

    public ItemPreset GetItemByID(string id)
    {
        foreach (ItemPreset item in availableItems)
        {
            if (item.id == id)
                return item;
        }

        return null;
    }

    #region Adding_Items

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
        ItemPreset item = GetItemByID(id);

        if (!item)
        {
            Debug.LogWarning($"Wrong ID ({id}) passed to function AddItemByID().");
            return;
        }

        AddItem(item, amount);
    }

    #endregion

    public bool HasItem(ItemPreset item)
    {
        foreach(Slot slot in slots)
        {
            if (slot.item == item)
                return true;
        }

        return false;
    }

    public bool HasItemCount(ItemPreset item, int count)
    {
        int missing = count;
        foreach(Slot slot in slots)
        {
            if(slot.item == item)
            {
                missing -= slot.count;
                if(missing <= 0)
                {
                    return true;
                }
            }
        }

        return (missing <= 0);
    }

    #region Removing_Items

    public void RemoveItem(ItemPreset item, int count)
    {

    }

    public void RemoveFromSlot(int slotIndex, int count)
    {
        slots[slotIndex].DecreaseCount(count);

        onContentsChanged?.Invoke();
    }

    #endregion

    #region Using_Items

    public bool IsItemUsable(ItemPreset item)
    {
        foreach(Type t in usableTypes)
        {
            if (item.GetType() == t)
                return true;
        }

        return false;
    }

    public bool IsItemInSlotUsable(int slotIndex)
    {
        ItemPreset inSlot = slots[slotIndex].item;
        if (!inSlot)
            return false;

        return IsItemUsable(inSlot);
    }

    public void UseItemAtSlot(int slotIndex)
    {
        ItemPreset itemAtIndex = slots[slotIndex].item;
        if (!itemAtIndex)
            return;

        if (!IsItemUsable(itemAtIndex))
            return;

        itemService.UseItem(itemAtIndex);
        RemoveFromSlot(slotIndex, 1);
    }

    public void UseHotkey(int key)
    {
        if (key < 1 || key > settings.hotkeyAmount)
            return;

        UseItemAtSlot(key - 1);
    }

    #endregion
}