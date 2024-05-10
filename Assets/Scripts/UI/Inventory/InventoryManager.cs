using UnityEngine;
using Chapter.Singleton;

public class InventoryManager : Singleton<InventoryManager>
{
    GameObject inventory;
    public bool isOpen;

    // Start - initialize and hide inventory
    public void OnStart()
    {
        inventory = GameObject.Find("Inventory");
        isOpen = false;
        inventory.SetActive(false); // hide inventory by default
    }
    // changing state of inventory window
    public void ChangeState()
    {
        inventory.SetActive(!inventory.activeInHierarchy);
        isOpen = !isOpen;

        // Disable player movement if inventory is opened
        if (isOpen) InputManager.Instance.GetInput().Disable();
        else InputManager.Instance.GetInput().Enable();
    }
}
