using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform slotsContainer, chestContainer;
    [SerializeField] GameObject chestUiObj;
    [SerializeField] GameObject slotPrefab;

    [SerializeField] TooltipUI tooltip;
    RectTransform tooltipTransform;
    bool tooltipShown = false;
    SlotUI tooltipSlot = null;
    [SerializeField] float tpOffset;

    [SerializeField] GameObject dragSlotObj;
    [SerializeField] Image dragSlotImage;
    [SerializeField] TextMeshProUGUI dragSlotText;
    RectTransform dragSlotRect;
    bool dragSlotShown = false;

    GridLayoutGroup grid;

    SlotUI[] slotUIs = null;
    SlotUI[] chestUIs = null;

    [Inject] readonly InventoryService service;

    void Start()
    {
        tooltipTransform = tooltip.GetComponent<RectTransform>();
        HideTooltip();

        chestUiObj.SetActive(false);

        SpawnSlots();

        SubEvents();
    }

    void SubEvents()
    {
        service.onContentsChanged += UpdateAllSlots;
        service.onContentsChanged += UpdateTooltip;

        service.onDragStart += ShowDragSlot;
        service.onDragEnd += HideDragSlot;

        service.onNewContainer += ShowChest;
        service.onContainerLost += HideChest;
    }

    void UnsubEvents()
    {
        service.onContentsChanged -= UpdateAllSlots;
        service.onContentsChanged -= UpdateTooltip;

        service.onDragStart -= ShowDragSlot;
        service.onDragEnd -= HideDragSlot;

        service.onNewContainer -= ShowChest;
        service.onContainerLost -= HideChest;
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

    void SpawnChestSlots()
    {
        int amount = service.currentCont.slots.Length;

        chestUIs = new SlotUI[amount];

        for(int i = 0; i < amount; i++)
        {
            SlotUI slotUI = Instantiate(slotPrefab, chestContainer).GetComponent<SlotUI>();
            slotUI.InitSlot(i, this, -1);
            chestUIs[i] = slotUI;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(chestContainer.GetComponent<RectTransform>());
    }

    void DestroyChestSlots()
    {
        if (chestUIs == null || chestUIs.Length == 0)
            return;

        // Go backwards to avoid indexing issues if array ever gets exchanged for a list
        for(int i = chestUIs.Length - 1; i >= 0; i--)
        {
            Destroy(chestUIs[i].gameObject);
        }

        chestUIs = null;
    }

    void ShowChest()
    {
        chestUiObj.SetActive(true);
        SpawnChestSlots();
        UpdateAllSlots();
    }

    void HideChest()
    {
        DestroyChestSlots();
        chestUiObj.SetActive(false);

        if (tooltipShown)
            HideTooltip();
    }

    bool IsChestSlot(SlotUI slot)
    {
        if (chestUIs == null)
            return false;

        foreach(SlotUI ui in chestUIs)
        {
            if (slot == ui)
                return true;
        }

        return false;
    }

    void UpdateSlot(int index, bool isChest)
    {
        Slot targetSlot = isChest ? service.currentCont.slots[index] : service.inventory.slots[index];
        SlotUI toUpdate = isChest ? chestUIs[index] : slotUIs[index];

        if (targetSlot.item != null)
        {
            toUpdate.ShowItem(targetSlot.item, targetSlot.count);
        }
        else
        {
            toUpdate.ClearUI();
        }
    }

    void UpdateAllSlots()
    {
        for(int i = 0; i < slotUIs.Length; i++)
        {
            UpdateSlot(i, false);
        }

        if (chestUIs != null)
        {
            for (int i = 0; i < chestUIs.Length; i++)
            {
                UpdateSlot(i, true);
            }
        }
    }

    void UpdateTooltip()
    {
        if (tooltipSlot == null)
            return;

        ShowTooltip(tooltipSlot);
    }

    public void ShowTooltip(SlotUI caller)
    {
        if (dragSlotShown)
            return;

        Slot slot = IsChestSlot(caller) ? service.currentCont.slots[caller.index] : service.inventory.slots[caller.index];

        ItemPreset item = slot.item;
        int count = slot.count;

        if (item != null && count > 0)
        {
            tooltip.UpdateText(item, count);

            tooltip.gameObject.SetActive(true);
            tooltipShown = true;
            tooltipSlot = caller;

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
        tooltipSlot = null;
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

    public void AssignHover(SlotUI caller)
    {
        service.slotHoverIndex = !caller ? null : caller.index;
        service.slotHoverChest = !caller ? false : IsChestSlot(caller);
    }

    #endregion

    private void OnDestroy()
    {
        UnsubEvents();
    }
}
