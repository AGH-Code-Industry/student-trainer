using UnityEngine;

public class Slot
{
    public ItemPreset item { get; private set; } = null;
    public int count { get; private set; } = 0;

    public int AssignItem(ItemPreset newItem, int newCount)
    {
        item = newItem;

        return SetCount(newCount);
    }

    public void ClearSlot()
    {
        item = null;
        count = 0;
    }

    // AssignItem, SetCount and IncreaseCount returns the amount of leftover items
    // (if the amount to be added exceeded the stack size)

    // DecreaseCount returns the amount of items remaining in the slot
    // (returns negative if the amount of items in the slot was lower than the requested decrease amount)

    public int SetCount(int newCount)
    {
        if(!Stackable())
        {
            count = 1;
            return newCount - 1;
        }

        if(newCount <= 0)
        {
            ClearSlot();
            return 0;
        }

        if(newCount < MaxStack())
        {
            count = newCount;
            return 0;
        }
        else
        {
            count = MaxStack();
            int remaining = newCount - MaxStack();
            if (remaining < 0)
                remaining = 0;

            return remaining;
        }
    }

    public int IncreaseCount(int amount)
    {
        if (!Stackable())
            return amount;

        int newCount = count + amount;

        if(newCount > MaxStack())
        {
            int remaining = newCount - MaxStack();
            count = MaxStack();
            return remaining;
        }
        else
        {
            count = newCount;
            return 0;
        }
    }

    public int DecreaseCount(int amount)
    {
        if(!Stackable())
        {
            ClearSlot();
            return amount - 1;
        }

        int newCount = count - amount;

        if(newCount > 0)
        {
            count = newCount;
            return count;
        }
        else
        {
            ClearSlot();
            int leftover = -newCount;
            return leftover;
        }
    }

    // Shortcut functions
    public int MaxStack() { return item.maxStackSize; }
    public bool Stackable() { return item.maxStackSize > 1; }
    public bool IsEmpty() { return item == null || count <= 0; }
}