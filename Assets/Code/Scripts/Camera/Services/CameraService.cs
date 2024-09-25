using System;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CameraService : Zenject.IInitializable
{
    public event Action<VirtualCameraType> ActiveCamera;
    public VirtualCameraType ActiveCameraType;

    private Dictionary<VirtualCameraType, Transform> camerasByTransform = new();
    public bool CanCameraMove = false;

    public Camera main { get; private set; }

    [Inject] private ResourceReader _resourceReader;

    public CinemachineCameraSettings Settings { get; private set; }

    public void Initialize()
    {
        main = Camera.main;
        Settings = _resourceReader.ReadSettings<CinemachineCameraSettings>();
    }

    public Camera GetMainCamera() => main;

    public void AddCameraTransform(VirtualCameraType type, Transform transform) => camerasByTransform.TryAdd(type, transform);

    public Transform GetCameraTransform(VirtualCameraType type)
    {
        if (camerasByTransform.TryGetValue(type, out var transform))
        {
            return transform;
        }
        return null;
    }

    public void SetActiveCamera(VirtualCameraType type)
    {
        ActiveCamera.Invoke(type);
        ActiveCameraType = type;
    }
}