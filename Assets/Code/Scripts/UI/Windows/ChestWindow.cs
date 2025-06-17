using UnityEngine;

public class ChestWindow : WindowUI
{
    public override string dataID { get; protected set; } = "ChestUI";
    [SerializeField] InventoryUI inventoryUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnWindowOpened()
    {
        inventoryUI.ShowChest();
    }

    public override void OnWindowClosed()
    {
        inventoryUI.HideChest();
    }
}
