using UnityEngine;

[CreateAssetMenu(fileName = "New Generic Enemy Settings", menuName = "Settings/Enemies/Generic Enemy")]
public class GenericEnemySettings : ScriptableObject
{
    public float health = 100f;
    public float moveSpeed = 5f;
    public ComboList attacks;

    [Header("Perception")]
    public float detectionRange = 10f;
    public float detectionFov = 80f;
}