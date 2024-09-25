using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


[RequireComponent(typeof(PlayerInput), typeof(CinemachineController))]
public class CameraRotationController : MonoBehaviour
{
    public readonly float ONE_DEGREE_TO_VIEWPORT = 1 / 360.0f;

    private float _lastXPosition;
    private Camera _mainCamera;

    [Inject] private CameraService _cameraService;
    private CinemachineController _cinemachineController;

    private void Start()
    {
        _mainCamera = Camera.main;
        _cinemachineController = GetComponent<CinemachineController>();
    }

    private void OnStartRotation()
    {
        if (_cameraService.ActiveCameraType == _cinemachineController.Type)
        {
            StartCoroutine("Rotate");
        }
    }

    private void OnStopRotation()
    {
        StopAllCoroutines();
    }

    IEnumerator Rotate()
    {
        Vector3 viewportPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        _lastXPosition = viewportPosition.x;
        while (true)
        {
            yield return null;
            viewportPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            var angle = (viewportPosition.x - _lastXPosition) / ONE_DEGREE_TO_VIEWPORT;
            transform.Rotate(Vector3.up, angle, Space.World);
            _lastXPosition = viewportPosition.x;
        }
    }
}