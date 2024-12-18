using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementService : IInitializable
{
    public bool frozen { get; private set; }

    Vector3 _lastLookTarget = Vector3.zero;

    [Inject] ResourceReader _reader;
    PlayerMovementSettings _settings;

    [Inject] readonly InputService _input;

    public void Initialize()
    {
        _settings = _reader.ReadSettings<PlayerMovementSettings>();
    }

    public Vector3 GetMovementVector()
    {
        if (frozen)
            return Vector3.zero;

        Vector2 vec = _input.movementVector;
        Vector3 movement = new Vector3(vec.x, 0, vec.y);
        movement *= _settings.movementSpeed;

        // Leaves room for modifying the vector
        // For example: wearing armor may reduce speed, being drunk may add variation to the vector, etc.

        return movement;
    }

    public Vector3 GetLookVector()
    {
        if (!frozen)
            _lastLookTarget = _input.globalLookTarget;

        return _lastLookTarget;
    }

    public float GetRotationSpeed() { return _settings.rotationSpeed; }

    public bool SetFreeze(bool _state) => frozen = _state;
    public bool Freeze() => SetFreeze(true);
    public bool Unfreeze() => SetFreeze(false);
}
