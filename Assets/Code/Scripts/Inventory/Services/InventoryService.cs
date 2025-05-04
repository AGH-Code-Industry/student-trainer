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
    public Container inventory { get; private set; }
    public Container currentCont { get; private set; } = null;

    public Slot draggedItem { get; private set; }
    public int dragOriginIndex;
    public int? slotHoverIndex = null;
    public bool slotHoverChest = false;

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

    public event Action onNewContainer, onContainerLost;

    // Types of items that can be used
    readonly Type[] usableTypes = { typeof(HealingItemPreset), typeof(BuffItemPreset) };

    public void Initialize()
    {
        settings = reader.ReadSettings<InventorySettings>();

        availableItems = (ItemPreset[])reader.ReadAllSettings<ItemPreset>();
        inventory = new Container(settings.inventorySize, "Inventory");

        draggedItem = new Slot();

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

    public void SetContainer(Container newCont)
    {
        currentCont = newCont;

        if (currentCont != null)
            onNewContainer?.Invoke();
        else
            onContainerLost?.Invoke();
    }

    public void ClearContainer()
    {
        SetContainer(null);
    }

    #region Adding_Items

    public void AddItem(ItemPreset item, int amount, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        bool stackable = item.maxStackSize > 1;
        int amountLeft = amount;

        for(int i = 0; i < cont.slots.Length; i++)
        {
            // amountLeft should never go below zero
            Assert.IsTrue(amountLeft >= 0);

            if(stackable)
            {
                // Add items to slot if it contains the same item
                if(cont.slots[i].item == item)
                {
                    amountLeft = cont.slots[i].IncreaseCount(amountLeft);
                    if (amountLeft == 0)
                        break;
                }
            }

            // Add item to slot if slot is free
            if (!cont.slots[i].item)
            {
                amountLeft = cont.slots[i].AssignItem(item, amountLeft);
                if (amountLeft == 0)
                    break;
            }
        }

        onContentsChanged?.Invoke();
    }

    // Only fills a single slot, then returns the amount left
    public int AddItemNoOverflow(ItemPreset item, int amount, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        bool stackable = item.maxStackSize > 1;
        int leftover = 0;

        // Doesn't stack, add to first free slot (if exists)
        if (!stackable)
        {
            int target = GetFirstFreeSlot(cont);
            if (target == -1)
                return amount;

            leftover = cont.slots[target].AssignItem(item, amount);
        }
        else
        {
            for (int i = 0; i < cont.slots.Length; i++)
            {
                if (cont.slots[i].IsEmpty())
                {
                    leftover = cont.slots[i].AssignItem(item, amount);
                    break;
                }
                else if (cont.slots[i].item == item && cont.slots[i].count < item.maxStackSize)
                {
                    leftover = cont.slots[i].IncreaseCount(amount);
                    break;
                }
            }
        }

        onContentsChanged?.Invoke();
        return leftover;
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

    #region Checking_Items

    public bool HasItem(ItemPreset item, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        foreach(Slot slot in cont.slots)
        {
            if (slot.item == item)
                return true;
        }

        return false;
    }

    public bool HasItemCount(ItemPreset item, int count, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        int missing = count;
        foreach(Slot slot in cont.slots)
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

    public int GetFirstFreeSlot(Container cont = null)
    {
        return GetFirstFreeAfterIndex(0, cont);
    }

    public int GetFirstFreeAfterIndex(int slotIndex, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        if (slotIndex >= cont.slots.Length - 1)
            return -1;

        for(int i = slotIndex; i < cont.slots.Length; i++)
        {
            if(cont.slots[i].IsEmpty())
            {
                return i;
            }
        }

        return -1;
    }

    public int GetAmountOfFreeSlots(Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        int freeSlots = 0;
        foreach(Slot slot in cont.slots)
        {
            if (slot.IsEmpty())
                freeSlots++;
        }

        return freeSlots;
    }

    #endregion

    #region Removing_Items

    public void RemoveItem(ItemPreset item, int count, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        bool stackable = item.maxStackSize > 1;
        int amountLeft = count;

        for (int i = 0; i < cont.slots.Length; i++)
        {
            if (cont.slots[i].item == item)
            {
                amountLeft = cont.slots[i].DecreaseCount(amountLeft);
                if (amountLeft == 0)
                    break;
            }
        }

        onContentsChanged?.Invoke();
    }

    public void RemoveFromSlot(int slotIndex, int count, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        cont.slots[slotIndex].DecreaseCount(count);

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

    public bool IsItemInSlotUsable(int slotIndex, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        ItemPreset inSlot = cont.slots[slotIndex].item;
        if (!inSlot)
            return false;

        return IsItemUsable(inSlot);
    }

    public void UseItemAtSlot(int slotIndex, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        ItemPreset itemAtIndex = cont.slots[slotIndex].item;
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

        Container target = slotHoverChest ? currentCont : inventory;
        if(target == null)
        {
            Debug.LogError("InventoryService error at function SlotClick: slotHoverChest variable is true but current container is null!");
            return;
        }

        if (click.button == MouseClickEvent.MouseButton.Left)
        {
            if (click.ctx.started)
            {
                if (click.shiftModifier)
                    FastTranferFromSlot((int)slotHoverIndex, target);
                else
                    DragItemFromSlot((int)slotHoverIndex, target);
            }
            else if (click.ctx.canceled)
            {
                if (slotHoverIndex == null)
                    DropItemIntoSlot(dragOriginIndex, target);
                else
                    DropItemIntoSlot((int)slotHoverIndex, target);
            }
        }
        else if (click.button == MouseClickEvent.MouseButton.Right)
        {
            if (click.ctx.performed)
                UseItemAtSlot((int)slotHoverIndex, target);
        }
        else if(click.button == MouseClickEvent.MouseButton.Middle)
        {
            if (click.ctx.performed)
                SplitStackAtIndex((int)slotHoverIndex, target);
        }
    }

    public void SplitStackAtIndex(int slotIndex, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        if (cont.slots[slotIndex].IsEmpty() || !cont.slots[slotIndex].Stackable() || cont.slots[slotIndex].count == 1)
            return;

        int closestEmpty = GetFirstFreeAfterIndex(slotIndex);
        if (closestEmpty == -1)
            return;

        int original = cont.slots[slotIndex].count;
        int hand = original / 2;
        int left = original - hand;

        ItemPreset item = cont.slots[slotIndex].item;
        cont.slots[slotIndex].SetCount(left);

        cont.slots[closestEmpty].AssignItem(item, hand);

        onContentsChanged?.Invoke();
    }

    public void FastTranferFromSlot(int slotIndex, Container cont = null)
    {
        Container destination;

        if (cont == null || cont == inventory)
        {
            cont = inventory;
            if (currentCont == null)
                return;

            destination = currentCont;
        }
        else if(cont != inventory)
        {
            destination = inventory;
        }
        else
        {
            Debug.LogError("Seemingly impossible option has occured in function FastTransferFromSlot (InventoryService)!");
            return;
        }

        ItemPreset item = cont.slots[slotIndex].item;
        int count = cont.slots[slotIndex].count;

        int leftover = AddItemNoOverflow(item, count, destination);

        if (leftover > 0)
        {
            // No more free slots in the inventory
            if (GetAmountOfFreeSlots(destination) == 0)
            {
                cont.slots[slotIndex].SetCount(leftover);
            }
            else
            {
                AddItemNoOverflow(item, leftover, destination);
                cont.slots[slotIndex].ClearSlot();
            }
        }
        else
        {
            cont.slots[slotIndex].ClearSlot();
        }

        onContentsChanged?.Invoke();
    }

    #region DraggingItems

    public void DragItemFromSlot(int slotIndex, Container cont = null)
    {
        if (cont == null)
            cont = inventory;

        ItemPreset item = cont.slots[slotIndex].item;
        int count = cont.slots[slotIndex].count;

        if (item == null || count == 0)
            return;

        draggedItem.AssignItem(item, count);
        cont.slots[slotIndex].ClearSlot();

        dragOriginIndex = slotIndex;

        onDragStart?.Invoke();
        onContentsChanged?.Invoke();
    }

    public void DropItemIntoSlot(int slotIndex, Container cont = null)
    {
        if (draggedItem.item == null || draggedItem.count == 0)
            return;

        if (cont == null)
            cont = inventory;

        onDragEnd?.Invoke();

        bool targetEmpty = cont.slots[slotIndex].item == null;
        if(targetEmpty)
        {
            cont.slots[slotIndex].AssignItem(draggedItem.item, draggedItem.count);
            draggedItem.ClearSlot();
            onContentsChanged?.Invoke();
            return;
        }

        bool sameItems = draggedItem.item == cont.slots[slotIndex].item;
        if(sameItems)
        {
            // Items in both slots are the same. Non-stackable: do nothing. Stackable: merge stacks.

            if (!draggedItem.Stackable())
            {
                draggedItem.ClearSlot();
                onContentsChanged?.Invoke();
                return;
            }

            int leftover = cont.slots[slotIndex].IncreaseCount(draggedItem.count);

            if(leftover > 0)
                cont.slots[dragOriginIndex].AssignItem(draggedItem.item, leftover);
        }
        else
        {
            ItemPreset replacedItem = cont.slots[slotIndex].item;
            int replacedCount = cont.slots[slotIndex].count;

            cont.slots[slotIndex].AssignItem(draggedItem.item, draggedItem.count);
            cont.slots[dragOriginIndex].AssignItem(replacedItem, replacedCount);
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