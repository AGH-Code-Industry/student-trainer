using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Settings", menuName = "Settings/Player Movement")]
public class PlayerMovementSettings : ScriptableObject
{
    // Standard movement speed. Would be good if it matched the walk animation, so the feet don't slide
    public float movementSpeed;
    // How fast the model rotates to face the current direction
    public float rotationSpeed;
}