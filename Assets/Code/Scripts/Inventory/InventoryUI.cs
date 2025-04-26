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

    [SerializeField] GameObject dragSlotObj;
    [SerializeField] Image dragSlotImage;
    [SerializeField] TextMeshProUGUI dragSlotText;
    RectTransform dragSlotRect;
    bool dragSlotShown = false;

    GridLayoutGroup grid;

    SlotUI[] slotUIs;

    [Inject] readonly InventoryService service;

    void Start()
    {
        SpawnSlots();

        service.onContentsChanged += UpdateAllSlots;
        service.onContentsChanged += UpdateTooltip;

        service.onDragStart += ShowDragSlot;
        service.onDragEnd += HideDragSlot;
    }

    void FixedUpdate()
    {
        if(tooltipShown)
        {
            tooltipTransform.anchoredPosition = GetTooltipNewAnchorPos();
        }
        if(dragSlotShown)
        {
            dragSlotRect.anchoredPosition = Input.mousePosition;
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

        grid = GetComponent<GridLayoutGroup>();
        dragSlotRect = dragSlotObj.GetComponent<RectTransform>();
        dragSlotRect.sizeDelta = grid.cellSize;

        HideDragSlot();
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
        if (dragSlotShown)
            return;

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

    public void ShowDragSlot()
    {
        dragSlotObj.SetActive(true);

        if(service.draggedItem.Stackable())
        {
            dragSlotText.gameObject.SetActive(true);
            dragSlotText.text = service.draggedItem.count.ToString();
        }
        else
        {
            dragSlotText.gameObject.SetActive(false);
        }

        dragSlotImage.sprite = service.draggedItem.item.icon;
        dragSlotShown = true;

        dragSlotRect.anchoredPosition = Input.mousePosition;
    }

    public void HideDragSlot()
    {
        dragSlotObj.SetActive(false);
        dragSlotShown = false;
    }

    #region SlotUI_Mediators

    public void AssignHoverIndex(int? index)
    {
        service.slotHoverIndex = index;
    }
    /*
    public void UseItemFromSlot(int index)
    {
        service.UseItemAtSlot(index);
    }

    public void TryMoveItem(int index)
    {
        service.DragItemFromSlot(index);
    }

    public void DropIntoSlot(int index)
    {
        service.DropItemIntoSlot(index);
    }
    */

    #endregion

    private void OnDestroy()
    {
        service.onContentsChanged -= UpdateAllSlots;
        service.onContentsChanged -= UpdateTooltip;
    }
}
