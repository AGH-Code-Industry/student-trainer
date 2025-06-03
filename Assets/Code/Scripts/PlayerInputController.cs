using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _input;
    [Inject] private EventBus _eventBus;

    void OnEnable()
    {
        _input = GetComponent<PlayerInput>();
        _input.onActionTriggered += OnAction;
    }

    void OnAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "MouseClick":
                if (!EventSystem.current.IsPointerOverGameObject())
                    _eventBus.Publish(new MouseClickUncaught(context));
                else
                    _eventBus.Publish(new MouseClickEvent(context));
                break;
            case "Run":
                _eventBus.Publish(new PlayerRun(context)); break;
            case "Move":
                _eventBus.Publish(new PlayerMove(context)); break;
            case "Dodge":
                _eventBus.Publish(new PlayerDodge(context)); break;
            case "Interact":
                _eventBus.Publish(new PlayerInteractEvent(context)); break;
            case "InventoryHotkey":
                _eventBus.Publish(new PlayerHotkey(context)); break;
        }
    }

    void OnDisable()
    {
        _input.onActionTriggered -= OnAction;

    }
}