using UnityEngine;

[CreateAssetMenu(fileName = "CinemachineCamera", menuName = "Settings/CinemachineCamera")]
public class CinemachineCameraSettings : ScriptableObject
{
    [Header("Move")]
    public float moveSpeed = 2.0f;

    [Header("Zoom")]
    public float zoomSpeed = 1;
    public float minZoom = 5;
    public float maxZoom = 13;
    public float rotationXAtMaxZoom = 50;
    public float rotationXAtMinZoom = 17.5f;
}