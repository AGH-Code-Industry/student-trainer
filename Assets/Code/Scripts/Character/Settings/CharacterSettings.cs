using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Settings/Character")]
public class CharacterSettings : ScriptableObject
{
    public new string name;
    public Sprite sprite;
}