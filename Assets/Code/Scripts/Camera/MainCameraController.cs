using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(PlayerInput))]
public class MainCameraController : MonoBehaviour
{
    [Inject] CameraService _cameraService;

    private void OnMove()
    {
        if (_cameraService.ActiveCameraType != VirtualCameraType.World)
            _cameraService.SetActiveCamera(VirtualCameraType.World);
    }

    private void OnBackToPlayer()
    {
        if (_cameraService.ActiveCameraType != VirtualCameraType.Player)
            _cameraService.SetActiveCamera(VirtualCameraType.Player);
    }
}