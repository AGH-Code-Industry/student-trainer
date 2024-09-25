using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(CinemachineController))]
public class CameraMoveController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    [Inject] private CameraService _cameraService;
    private CinemachineController _cinemachineController;
    private CinemachineCameraSettings _settings;

    private void Start()
    {
        _cinemachineController = GetComponent<CinemachineController>();
        _settings = _cameraService.Settings;
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_cameraService.ActiveCameraType == _cinemachineController.Type)
        {
            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            Vector3 right = new Vector3(transform.right.x, 0, transform.right.z);
            Vector3 moveDirection = (forward * direction.y) + (right * direction.x);
            Vector3 moveTo = moveDirection * _settings.moveSpeed + transform.position;

            transform.LeanMove(moveTo, Time.fixedDeltaTime);
        }
    }
}