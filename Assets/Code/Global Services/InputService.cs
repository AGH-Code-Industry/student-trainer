using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputService : IInitializable
{

    #region Player Input
    public event Action<Vector3> GlobalLookTargetChange;
    private Vector3 _globalLookTarget;
    public Vector3 GlobalLookTarget
    {
        get => _globalLookTarget;
        set
        {
            _globalLookTarget = value;
            GlobalLookTargetChange?.Invoke(value);
        }
    }

    public Vector2 movementVector { get; set; }
    public Vector3 MouseDownPosition { get; set; }

    #endregion

    public void Initialize()
    {
        return;
    }
}