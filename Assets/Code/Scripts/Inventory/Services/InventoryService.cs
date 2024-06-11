using UnityEngine;
using Zenject;

public class InventoryService : MonoBehaviour
{
    GameObject inventory;
    public bool isOpen;

    [Inject]
    private readonly InputService input;

    // Start - initialize and hide inventory
    public void Start()
    {
        isOpen = false;
        inventory.SetActive(false); // hide inventory by default
    }
    // changing state of inventory window
    public void ChangeState()
    {
        inventory.SetActive(!inventory.activeInHierarchy);
        isOpen = !isOpen;

        // Disable player movement if inventory is opened
        input.SetActive(isOpen);
    }


}
