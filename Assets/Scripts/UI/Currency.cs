using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Currency : MonoBehaviour
{
    public int amount = 0;

    public TMP_Text currencyDisplay;

    public void Update()
    {
        currencyDisplay.text = amount.ToString();
    }

    public bool TryBuy(int price)
    {
        if (price > amount)
        {
            Debug.Log("Too expencive");
            return false;
        }
        amount -= price;
        return true;
    }
}
