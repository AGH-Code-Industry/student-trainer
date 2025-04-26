/*

    FEATURES TO ADD:
    - Dropping items on the ground
    - Splitting item stacks in half by pressing the scroll wheel?

*/

using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class InventoryService : IInitializable, IDisposable
{
    public InventorySettings settings { get; private set; }

    public ItemPreset[] availableItems { get; private set; }
    public Slot[] slots { get; private set; }

    public Slot draggedItem { get; private set; }
    public int dragOriginIndex;
    public int? slotHoverIndex = null;

    public bool IsDraggingItem
    {
        get { return draggedItem.item != null; }
        private set { }
    }

    [Inject] readonly ResourceReader reader;
    [Inject] readonly ItemUsingService itemService;
    [Inject] readonly EventBus eventBus;

    // Pass slot's index as argument
    //public Action<int> onSlotContentsChanged;
    public event Action onContentsChanged;

    public event Action onDragStart, onDragEnd;

    // Types of items that can be used
    readonly Type[] usableTypes = { typeof(HealingItemPreset), typeof(BuffItemPreset) };

    public void Initialize()
    {
        settings = reader.ReadSettings<InventorySettings>();

        availableItems = (ItemPreset[])reader.ReadAllSettings<ItemPreset>();
        slots = new Slot[settings.inventorySize];

        draggedItem = new Slot();

        for(int i = 0; i < settings.inventorySize; i++)
        {
            slots[i] = new Slot();
        }

        eventBus.Subscribe<PlayerHotkey>(UseHotkey);

        eventBus.Subscribe<MouseClickEvent>(SlotClick);
        eventBus.Subscribe<MouseClickUncaught>(SlotClick);
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

    public int GetFirstFreeAfterIndex(int slotIndex)
    {
        if (slotIndex >= slots.Length - 1)
            return -1;

        for(int i = slotIndex; i < slots.Length; i++)
        {
            if(slots[i].IsEmpty())
            {
                return i;
            }
        }

        return -1;
    }

    #region Removing_Items

    public void RemoveItem(ItemPreset item, int count)
    {
        bool stackable = item.maxStackSize > 1;
        int amountLeft = count;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                amountLeft = slots[i].DecreaseCount(amountLeft);
                if (amountLeft == 0)
                    break;
            }
        }

        onContentsChanged?.Invoke();
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

    public void UseHotkey(PlayerHotkey hotkey)
    {
        if (!hotkey.ctx.performed)
            return;

        if (hotkey.number < 1 || hotkey.number > settings.hotkeyAmount)
            return;

        UseItemAtSlot(hotkey.number - 1);
    }

    #endregion

    public void SlotClick(MouseClickEvent click)
    {
        if(click.GetType() == typeof(MouseClickUncaught))
        {
            if(slotHoverIndex == null && IsDraggingItem)
                DropItemIntoSlot(dragOriginIndex);

            return;
        }

        if (slotHoverIndex == null && !IsDraggingItem)
            return;

        if (click.button == MouseClickEvent.MouseButton.Left)
        {
            if (click.ctx.started)
            {
                DragItemFromSlot((int)slotHoverIndex);
            }
            else if (click.ctx.canceled)
            {
                if (slotHoverIndex == null)
                    DropItemIntoSlot(dragOriginIndex);
                else
                    DropItemIntoSlot((int)slotHoverIndex);
            }
        }
        else if (click.button == MouseClickEvent.MouseButton.Right)
        {
            if (click.ctx.performed)
                UseItemAtSlot((int)slotHoverIndex);
        }
        else if(click.button == MouseClickEvent.MouseButton.Middle)
        {
            if (click.ctx.performed)
                SplitStackAtIndex((int)slotHoverIndex);
        }
    }

    public void SplitStackAtIndex(int slotIndex)
    {
        if (slots[slotIndex].IsEmpty() || !slots[slotIndex].Stackable() || slots[slotIndex].count == 1)
            return;

        int closestEmpty = GetFirstFreeAfterIndex(slotIndex);
        if (closestEmpty == -1)
            return;

        int original = slots[slotIndex].count;
        int hand = original / 2;
        int left = original - hand;

        ItemPreset item = slots[slotIndex].item;
        slots[slotIndex].SetCount(left);

        slots[closestEmpty].AssignItem(item, hand);

        onContentsChanged?.Invoke();
    }

    #region DraggingItems

    public void DragItemFromSlot(int slotIndex)
    {
        ItemPreset item = slots[slotIndex].item;
        int count = slots[slotIndex].count;

        if (item == null || count == 0)
            return;

        draggedItem.AssignItem(item, count);
        slots[slotIndex].ClearSlot();

        dragOriginIndex = slotIndex;

        onDragStart?.Invoke();
        onContentsChanged?.Invoke();
    }

    public void DropItemIntoSlot(int slotIndex)
    {
        if (draggedItem.item == null || draggedItem.count == 0)
            return;

        onDragEnd?.Invoke();

        bool targetEmpty = slots[slotIndex].item == null;
        if(targetEmpty)
        {
            slots[slotIndex].AssignItem(draggedItem.item, draggedItem.count);
            draggedItem.ClearSlot();
            onContentsChanged?.Invoke();
            return;
        }

        bool sameItems = draggedItem.item == slots[slotIndex].item;
        if(sameItems)
        {
            // Items in both slots are the same. Non-stackable: do nothing. Stackable: merge stacks.

            if (!draggedItem.Stackable())
            {
                draggedItem.ClearSlot();
                onContentsChanged?.Invoke();
                return;
            }

            int leftover = slots[slotIndex].IncreaseCount(draggedItem.count);

            if(leftover > 0)
                slots[dragOriginIndex].AssignItem(draggedItem.item, leftover);
        }
        else
        {
            ItemPreset replacedItem = slots[slotIndex].item;
            int replacedCount = slots[slotIndex].count;

            slots[slotIndex].AssignItem(draggedItem.item, draggedItem.count);
            slots[dragOriginIndex].AssignItem(replacedItem, replacedCount);
        }

        draggedItem.ClearSlot();
        onContentsChanged?.Invoke();
    }

    //public bool IsDraggingItem() { return draggedItem.item != null; }

    #endregion

    public void Dispose()
    {
        eventBus.Unsubscribe<PlayerHotkey>(UseHotkey);
        eventBus.Unsubscribe<MouseClickEvent>(SlotClick);
        eventBus.Unsubscribe<MouseClickUncaught>(SlotClick);
    }
}