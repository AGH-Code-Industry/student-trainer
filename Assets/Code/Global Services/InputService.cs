using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Collections.Generic;

public class InputService : MonoBehaviour, IInitializable, IDisposable
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

    GameControls gameControls;
    PlayerInput playerInput;

    SortedSet<RegisteredInputConsumer> inputConsumers = new SortedSet<RegisteredInputConsumer>(new ConsumerPriorityDescending());

    public void Initialize()
    {
        gameControls = new GameControls();
        //InputSystem.onActionChange += OnAction;
        playerInput = gameObject.AddComponent<PlayerInput>();
        playerInput.actions = gameControls.asset;
        playerInput.SwitchCurrentActionMap("Player");
        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        playerInput.onActionTriggered += OnAction;
    }

    bool ConsumerExists(IInputConsumer consumer)
    {
        foreach (RegisteredInputConsumer reg in inputConsumers)
        {
            if (reg.consumer == consumer)
                return true;
        }

        return false;
    }

    public void RegisterConsumer(IInputConsumer toRegister, List<string> actions, bool uncaughtOnly = true)
    {
        if (ConsumerExists(toRegister))
            return;

        inputConsumers.Add(new RegisteredInputConsumer(toRegister, actions, uncaughtOnly));
    }

    public void RemoveConsumer(IInputConsumer toRemove)
    {
        inputConsumers.RemoveWhere(
            (RegisteredInputConsumer reg) =>
            {
                return reg.consumer == toRemove;
            }
        );
    }

    void OnAction(InputAction.CallbackContext ctx)
    {
        bool caught = false;
        foreach (RegisteredInputConsumer consumer in inputConsumers)
        {
            if(consumer.actions.Contains(ctx.action.name))
            {
                bool result = false;

                if(consumer.uncaughtOnly)
                {
                    if (!caught)
                        result = consumer.consumer.ConsumeInput(ctx);
                }
                else
                {
                    result = consumer.consumer.ConsumeInput(ctx);
                }

                if (caught == false && result == true)
                    caught = true;
            }
        }
    }

    public void Dispose()
    {
        playerInput.onActionTriggered -= OnAction;
    }

    class RegisteredInputConsumer
    {
        public int priority;
        public IInputConsumer consumer;
        public List<string> actions;
        public bool uncaughtOnly;

        public RegisteredInputConsumer(IInputConsumer _consumer, List<string> _actions, bool _uncaughtOnly)
        {
            consumer = _consumer;
            priority = consumer.priority;
            actions = _actions;
            uncaughtOnly = _uncaughtOnly;
        }
    }

    class ConsumerPriorityDescending : IComparer<RegisteredInputConsumer>
    {
        public int Compare(RegisteredInputConsumer a, RegisteredInputConsumer b)
        {
            int result = b.priority.CompareTo(a.priority);

            if(result == 0 && !ReferenceEquals(a, b))
            {
                return a.GetHashCode().CompareTo(b.GetHashCode());
            }

            return result;
        }
    }
}