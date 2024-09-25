using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineFollowZoom), typeof(CinemachineController))]
public class CameraZoomController : MonoBehaviour
{
    public float ZOOM_TO_ANGLE { get; private set; }

    [Inject] private CameraService _cameraService;

    private CinemachineFollowZoom _cameraZoom;
    private CinemachineController _cinemachineController;
    private CinemachineCameraSettings _settings;

    private void Start()
    {
        _settings = _cameraService.Settings;
        ZOOM_TO_ANGLE = (_settings.rotationXAtMaxZoom - _settings.rotationXAtMinZoom) / (_settings.maxZoom - _settings.minZoom);
        _cameraZoom = GetComponent<CinemachineFollowZoom>();
        _cinemachineController = GetComponent<CinemachineController>();
        SetZoom(_settings.maxZoom);
        transform.LeanRotateX(_settings.rotationXAtMaxZoom, 0);
    }

    private void OnZoom()
    {
        if (_cameraService.ActiveCameraType == _cinemachineController.Type)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            var zoom = ClampZoom(_cameraZoom.m_Width - (scroll * _settings.zoomSpeed));
            SetZoom(zoom);
            transform.LeanRotateX(_settings.rotationXAtMaxZoom - ((_settings.maxZoom - zoom) * ZOOM_TO_ANGLE), 0.2f);
        }
    }

    private void SetZoom(float zoom) => _cameraZoom.m_Width = zoom;

    private float ClampZoom(float zoom)
    {
        if (zoom < _settings.minZoom)
            return _settings.minZoom;
        if (zoom > _settings.maxZoom)
            return _settings.maxZoom;
        return zoom;
    }
}