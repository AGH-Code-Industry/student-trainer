using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> inventoryItems = new List<string>();

    public void ItemCollected(string item)
    {
        inventoryItems.Add(item);
        Debug.Log(string.Join("", inventoryItems));
    }
}
