using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsSO", menuName = "CharacterStatsSO", order = 0)]
public class CharacterStatsSO : ScriptableObject
{
    public float baseHealth;
    public float baseStamina;
    public float baseAttackDamage;
    public float resistances;

}