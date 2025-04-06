using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;

    const string colorStart = "<color=#AAAAAA>", colorEnd = "</color>";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateText(ItemPreset itemToShow, int amountToShow)
    {
        string toShow = itemToShow.name;

        if(itemToShow.maxStackSize > 1)
        {
            toShow += $" {colorStart}x {amountToShow}{colorEnd}";
        }

        nameText.text = toShow;
    }
}
