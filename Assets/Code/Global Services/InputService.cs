using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputService : IInitializable, IDisposable
{
    private InputActionAsset input;

    public void Initialize()
    {
        input = Resources.Load<GameControlsReference>("input").input;
    }

    public void Enable() => input.Enable();
    public void Disable() => input.Disable();
    public void SetActive(bool isActive)
    {
        if (isActive) Enable();
        else Disable();
    }

    public void Dispose()
    {
    }
}