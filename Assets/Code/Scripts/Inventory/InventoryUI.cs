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

    [SerializeField] TooltipUI tooltip;
    RectTransform tooltipTransform;
    bool tooltipShown = false;
    int tooltipIndex = -1;
    [SerializeField] float tpOffset;

    SlotUI[] slotUIs;

    [Inject] readonly InventoryService service;

    void Start()
    {
        SpawnSlots();

        service.onContentsChanged += UpdateAllSlots;
        service.onContentsChanged += UpdateTooltip;
    }

    void FixedUpdate()
    {
        if(tooltipShown)
        {
            tooltipTransform.anchoredPosition = GetTooltipNewAnchorPos();
        }
    }

    Vector2 GetTooltipNewAnchorPos()
    {
        // Could be changed to use the new input system, but I don't know if it's neccessary
        Vector2 newPos = Input.mousePosition;

        newPos.x += tpOffset;
        newPos.y += tpOffset;

        return newPos;
    }

    void SpawnSlots()
    {
        tooltipTransform = tooltip.GetComponent<RectTransform>();
        HideTooltip();

        int amount = service.settings.inventorySize;
        int hotkeys = service.settings.hotkeyAmount;

        slotUIs = new SlotUI[amount];

        for(int i = 0; i < amount; i++)
        {
            SlotUI slotUI = Instantiate(slotPrefab, slotsContainer).GetComponent<SlotUI>();
            int currentHotkey = i + 1 <= hotkeys ? i + 1 : -1;

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

    void UpdateTooltip()
    {
        if (tooltipIndex < 0)
            return;

        ShowTooltip(tooltipIndex);
    }

    public void ShowTooltip(int slotIndex)
    {
        ItemPreset item = service.slots[slotIndex].item;
        int count = service.slots[slotIndex].count;

        if (item != null && count > 0)
        {
            tooltip.UpdateText(item, count);

            tooltip.gameObject.SetActive(true);
            tooltipShown = true;
            tooltipIndex = slotIndex;

            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTransform);
            tooltipTransform.anchoredPosition = GetTooltipNewAnchorPos();
        }
        else
        {
            HideTooltip();
        }
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
        tooltipShown = false;
        tooltipIndex = -1;
    }

    // SlotUI mediator functions

    public void UseItemFromSlot(int index)
    {
        service.UseItemAtSlot(index);
    }



    private void OnDestroy()
    {
        service.onContentsChanged -= UpdateAllSlots;
        service.onContentsChanged -= UpdateTooltip;
    }
}
