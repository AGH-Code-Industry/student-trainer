using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Settings/Items/Basic", order = 0)]
public class ItemPreset : ScriptableObject
{
    [Tooltip("The id field is used by the game logic to reference an item. Try to keep the id the same after establishing it once.")]
    public string id = "item_id";
    public new string name = "Item Name";
    public Sprite icon;

    [TextArea]
    public string description;

    [Tooltip("If set to 1 or less, the item won't stack.")]
    public int maxStackSize = 1;
}