using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [SerializeField] InventoryUI invUI = null;

    void Start()
    {
        if(!invUI)
            invUI = FindObjectOfType<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
