using UnityEngine;

[CreateAssetMenu(fileName = "New Standard Enemy", menuName = "Settings/Enemies/Standard Enemy")]
public class StandardEnemySettings : ScriptableObject
{
    public float movementSpeed = 3f;

    public float detectionRange = 15f;
    public float detectionFov = 90f;
}