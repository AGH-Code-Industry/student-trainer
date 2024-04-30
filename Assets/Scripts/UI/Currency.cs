using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Currency : MonoBehaviour
{
    public static Currency instance;
    public TMP_Text currencyDisplay;
    public Canvas currencyCanvas;

    private string coinSprite = "<sprite name=\"Coin\"> ";
    private int currentAmount = 0;


    private void Awake()
    {
        instance = this;
        currencyDisplay.text = coinSprite + currentAmount.ToString();
    }

    private void Start()
    {
        currencyDisplay.text = coinSprite + currentAmount.ToString();
    }


    public bool TryBuy(int price)
    {
        if (price > currentAmount)
        {
            Debug.Log("Too expencive");
            return false;
        }
        currentAmount -= price;
        currencyDisplay.text = coinSprite + currentAmount.ToString();
        return true;
    }

    public void AddMoney(int moneyToAdd)
    {
        currentAmount += moneyToAdd;
        currencyDisplay.text = coinSprite + currentAmount.ToString();
    }
}
