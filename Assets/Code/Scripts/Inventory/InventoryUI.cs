using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform slotsContainer;
    [SerializeField] GameObject slotPrefab;

    [Space]

    [SerializeField] TMP_InputField itemidInput;
    [SerializeField] TMP_InputField amountInput;

    SlotUI[] slotUIs;

    [Inject] readonly InventoryService service;

    // Start is called before the first frame update
    void Start()
    {
        SpawnSlots();

        //service.onSlotContentsChanged += UpdateSlot;
        service.onContentsChanged += UpdateAllSlots;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSlots()
    {
        int amount = service.settings.inventorySize;
        int hotkeys = service.settings.hotkeyAmount;

        slotUIs = new SlotUI[amount];

        for(int i = 0; i < amount; i++)
        {
            SlotUI slotUI = Instantiate(slotPrefab, slotsContainer).GetComponent<SlotUI>();
            int currentHotkey = i + 1 <= hotkeys ? i + 1 : -1;
            //if (currentHotkey == 10)
                //currentHotkey = 0;

            slotUI.InitSlot(i, this, currentHotkey);
            slotUIs[i] = slotUI;
        }
    }

    void UpdateSlot(int index)
    {
        Slot toUpdate = service.slots[index];

        if (toUpdate.item != null)
        {
            slotUIs[index].ShowItem(toUpdate.item, toUpdate.count);
        }
        else
        {
            slotUIs[index].ClearUI();
        }
    }

    void UpdateAllSlots()
    {
        for(int i = 0; i < slotUIs.Length; i++)
        {
            UpdateSlot(i);
        }
    }

    private void OnDestroy()
    {
        //service.onSlotContentsChanged -= UpdateSlot;
        service.onContentsChanged -= UpdateAllSlots;
    }

    public void TestAddItem()
    {
        if (string.IsNullOrEmpty(itemidInput.text))
            return;

        string id = itemidInput.text;

        int amount = 1;
        if(!string.IsNullOrEmpty(amountInput.text))
            amount = int.Parse(amountInput.text);

        service.AddItemByID(id, amount);
    }

    // SlotUI mediator functions

    public void UseItemFromSlot(int index)
    {
        service.UseItemAtSlot(index);
    }
}
