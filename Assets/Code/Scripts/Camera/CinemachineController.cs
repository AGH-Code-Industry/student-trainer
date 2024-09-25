using System.Collections;
using Cinemachine;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineController : MonoBehaviour
{
    public VirtualCameraType Type => _type;
    [SerializeField] private VirtualCameraType _type;
    [Inject] private CameraService _cameraService;

    private CinemachineVirtualCamera _cinemachineVirtual;
    private ICinemachineTransition _transition = null;

    private void Awake()
    {
        _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        if (TryGetComponent<ICinemachineTransition>(out var transition))
        {
            _transition = transition;
        }
        _cameraService.AddCameraTransform(_type, transform);
        _cameraService.ActiveCamera += OnActiveCamera;
    }

    private void OnActiveCamera(VirtualCameraType type)
    {
        var isActive = _type == type;

        if (isActive)
        {
            _transition?.Execute();
        }

        _cinemachineVirtual.enabled = isActive;
    }

    private void OnDestroy()
    {
        _cameraService.ActiveCamera -= OnActiveCamera;
    }
}