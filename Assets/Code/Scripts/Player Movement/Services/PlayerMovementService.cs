using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementService : IInitializable
{
    [Inject] ResourceReader _reader;
    PlayerMovementSettings _settings;

    public void Initialize()
    {
        _settings = _reader.ReadSettings<PlayerMovementSettings>();
    }

    public Vector3 GetMovementVector(Vector2 playerInput)
    {
        Vector3 movement = new Vector3(playerInput.x, 0, playerInput.y);
        movement *= _settings.movementSpeed;

        // Leaves room for modifying the vector
        // For example: wearing armor may reduce speed, being drunk may add variation to the vector, etc.

        return movement;
    }

    public float GetRotationSpeed()
    {
        return _settings.rotationSpeed;
    }
}
