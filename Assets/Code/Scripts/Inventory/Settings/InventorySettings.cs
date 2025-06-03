using UnityEngine;

[CreateAssetMenu(fileName = "InventorySettings", menuName = "Settings/Inventory")]
public class InventorySettings : ScriptableObject
{
    public int inventorySize = 20;
    [Tooltip("Amount of slots that will be accessible by hotkeys (1-9). Keep between 0 and 9.")]
    public int hotkeyAmount = 5;
}