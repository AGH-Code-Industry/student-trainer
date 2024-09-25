using UnityEngine;
using Zenject;

public class WorldCinemachineTransition : MonoBehaviour, ICinemachineTransition
{
    [Inject] private CameraService _cameraService;

    public void Execute()
    {
        var playerTransform = _cameraService.GetCameraTransform(VirtualCameraType.Player);
        transform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
    }
}