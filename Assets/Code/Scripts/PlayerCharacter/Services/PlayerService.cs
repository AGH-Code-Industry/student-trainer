using System;
using UnityEngine;
using Zenject;

public class PlayerService : IInitializable, IDisposable
{
    public readonly float MaxHealth = 100;
    public event Action<float> HealthChange;
    private float _health = 100;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Math.Clamp(value, 0, MaxHealth);
            HealthChange?.Invoke(value);
        }
    }

    public bool frozen { get; private set; }
    public bool IsRunning { get; private set; }
    public Vector3 PlayerPosition { get; set; }

    Vector3 _lastLookTarget = Vector3.zero;

    [Inject] readonly ResourceReader _reader;
    [Inject] readonly EventBus _eventBus;
    [Inject] readonly InputService _input;

    private PlayerMovementSettings _settings;
    private Vector2 _movementVector;

    private readonly float gravity = Physics.gravity.y;
    float gravIntegral = 0f;
    bool isPlayerGrounded = true;
    LayerMask groundMask = LayerMask.GetMask("Ground");

    public void Initialize()
    {
        _settings = _reader.ReadSettings<PlayerMovementSettings>();

        _eventBus.Subscribe<PlayerMove>(OnMove);
        _eventBus.Subscribe<PlayerRun>(OnRun);
    }

    public void CheckIfGrounded()
    {
        isPlayerGrounded = Physics.CheckSphere(PlayerPosition, 0.1f, groundMask);
    }

    public Vector3 GetMovementVector()
    {
        if (frozen)
            return Vector3.zero;

        if(isPlayerGrounded)
        {
            gravIntegral = 0f;
        }
        else
        {
            gravIntegral += gravity * Time.deltaTime;
        }

        Vector2 vec = _movementVector;
        Vector3 movement = new Vector3(vec.x, 0, vec.y);
        movement *= IsRunning ? _settings.runSpeed : _settings.walkSpeed;
        movement.y = gravIntegral;

        // Leaves room for modifying the vector
        // For example: wearing armor may reduce speed, being drunk may add variation to the vector, etc.

        return movement;
    }

    public Vector3 GetLookVector()
    {
        if (!frozen)
            _lastLookTarget = _input.GlobalLookTarget;

        return _lastLookTarget;
    }

    public float GetRotationSpeed() { return _settings.rotationSpeed; }

    public bool SetFreeze(bool _state) => frozen = _state;
    public bool Freeze() => SetFreeze(true);
    public bool Unfreeze() => SetFreeze(false);

    private void OnMove(PlayerMove playerMove)
    {
        _movementVector = playerMove.move;
    }

    private void OnRun(PlayerRun playerRun)
    {
        if (playerRun.ctx.started || playerRun.ctx.performed) IsRunning = true;
        if (playerRun.ctx.canceled) IsRunning = false;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<PlayerMove>(OnMove);
        _eventBus.Unsubscribe<PlayerRun>(OnRun);
    }
}
