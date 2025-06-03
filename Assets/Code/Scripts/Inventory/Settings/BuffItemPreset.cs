using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Settings/Items/Buff", order = 2)]
public class BuffItemPreset : ItemPreset
{
    public int attackPower, armor;
}