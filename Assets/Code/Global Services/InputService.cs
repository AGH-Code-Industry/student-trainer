using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputService : IInitializable
{
    public Vector3 globalLookTarget { get; private set; }
    public Vector2 movementVector { get; private set; }

    public void Initialize()
    {
        return;
    }

    public void SetLookTarget(Vector3 _pos) => globalLookTarget = _pos;
    public void SetMovementVector(Vector2 _vec) => movementVector = _vec;
}