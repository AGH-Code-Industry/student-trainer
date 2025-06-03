using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Settings/Items/Healing", order = 1)]
public class HealingItemPreset : ItemPreset
{
    public float healAmount;
}