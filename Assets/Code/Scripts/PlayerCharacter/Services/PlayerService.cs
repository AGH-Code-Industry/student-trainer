using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerService : IInitializable, IDisposable, IInputConsumer
{
    public readonly float MaxHealth = 100;
    public event Action<float> HealthChange;
    private float _health = 100;


    bool isDead = false;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = Math.Clamp(value, 0, MaxHealth);
            HealthChange?.Invoke(_health);

            // Temporary death logic
            if(_health <= 0 && !isDead)
            {
                isDead = true;
                MonoBehaviour.FindAnyObjectByType<PlayerAnimationController>().PlayAnimation("Death");
                MonoBehaviour.FindAnyObjectByType<UiManager>().OpenWindow("DeathScreen");
            }
        }
    }

    public readonly SystemFreezer freezer = new SystemFreezer();

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

        _eventBus.Subscribe<LevelChangeBegin>(FreezeLevelChange);
        _eventBus.Subscribe<LevelChangeFinish>(UnfreezeLevelChange);

        List<string> wantedActions = new List<string>();
        wantedActions.Add("Move");
        wantedActions.Add("Run");
        _input.RegisterConsumer(this, wantedActions, true);
    }

    public void CheckIfGrounded()
    {
        isPlayerGrounded = Physics.CheckSphere(PlayerPosition, 0.1f, groundMask);
    }

    public Vector3 GetMovementVector()
    {
        if (freezer.Frozen)
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
        if (!freezer.Frozen)
            _lastLookTarget = _input.GlobalLookTarget;

        return _lastLookTarget;
    }

    public float GetRotationSpeed() { return _settings.rotationSpeed; }

    public void FreezeLevelChange(LevelChangeBegin beingEvent) => freezer.Freeze("levelChange");
    public void UnfreezeLevelChange(LevelChangeFinish finishEvent) => freezer.Unfreeze("levelChange");

    public void Dispose()
    {
        _eventBus.Unsubscribe<LevelChangeBegin>(FreezeLevelChange);
        _eventBus.Unsubscribe<LevelChangeFinish>(UnfreezeLevelChange);

        _input.RemoveConsumer(this);
    }

    public int priority { get; } = 1;

    public bool ConsumeInput(InputAction.CallbackContext context)
    {
        if(context.action.name == "Move")
        {
            _movementVector = context.ReadValue<Vector2>();
        }
        else if(context.action.name == "Run")
        {
            if (context.started || context.performed) IsRunning = true;
            else if (context.canceled) IsRunning = false;
        }

        if (freezer.Frozen)
            return false;
        else
            return true;
    }
}
