using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI hotkeyText;

    public int index { get; private set; } = -1;
    InventoryUI invUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSlot(int index, InventoryUI invUI, int hotkey = -1)
    {
        this.index = index;
        this.invUI = invUI;

        ClearUI();

        if(hotkey >= 0)
        {
            hotkeyText.gameObject.SetActive(true);
            hotkeyText.text = hotkey.ToString();
        }
        else
        {
            hotkeyText.gameObject.SetActive(false);
        }
    }

    public void ShowItem(ItemPreset item, int amount)
    {
        iconImage.gameObject.SetActive(true);
        iconImage.sprite = item.icon;

        if(item.maxStackSize > 1)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.gameObject.SetActive(false);
        }
    }

    public void ClearUI()
    {
        iconImage.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }

    #region Pointer_Events

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // RMB to use item
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            invUI.UseItemFromSlot(index);
        }
    }

    #endregion
}
