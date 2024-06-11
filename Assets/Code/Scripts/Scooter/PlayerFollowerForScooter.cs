using UnityEngine;

public class PlayerFollowerForScooter : MonoBehaviour
{
    void Update()
    {
        if (transform.parent == null) return;

        Quaternion newRotation = transform.parent.rotation * Quaternion.Euler(0, -90, 0);
        transform.position = transform.parent.position;
        transform.rotation = newRotation;
    }
}