using System;
using UnityEngine;
using Zenject;

public class PlayerMovementService : IInitializable
{
    public event Action<float> Speed;

    private PlayerMovementSetting setting;
    private PlayerMovementType actualPlayerMovement;

    [Inject] private ResourceReader _resourceReader;

    public void Initialize()
    {
        setting = _resourceReader.ReadSettings<PlayerMovementSetting>();
    }

    public Vector3 GetSpeed(Vector3 direction)
    {
        if (actualPlayerMovement == PlayerMovementType.Run)
            return direction * setting.speed;
        return direction * setting.speed * 0.5f;
    }

    public void SetSpeed(float newSpeed)
    {
        setting.speed = newSpeed;
        Speed?.Invoke(newSpeed);
    }
}
