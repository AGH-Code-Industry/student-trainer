using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI hotkeyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSlot(int hotkey = -1)
    {
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
}
