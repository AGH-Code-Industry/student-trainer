/*
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : BaseInputEvent
{
    public Vector2 move;

    public PlayerMove(InputAction.CallbackContext ctx) : base(ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }
}

public class PlayerRun : BaseInputEvent
{
    public PlayerRun(InputAction.CallbackContext ctx) : base(ctx) { }
}

public class PlayerDodge : BaseInputEvent
{
    public PlayerDodge(InputAction.CallbackContext ctx) : base(ctx) { }
}

public class MouseClickEvent : BaseInputEvent
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }
    public MouseButton button;
    public bool shiftModifier = false;

    public MouseClickEvent(InputAction.CallbackContext ctx) : base(ctx)
    {
        shiftModifier = Keyboard.current.leftShiftKey.isPressed;

        switch (ctx.control.name)
        {
            case "leftButton":
                button = MouseButton.Left; break;
            case "rightButton":
                button = MouseButton.Right; break;
            case "middleButton":
                button = MouseButton.Middle; break;

            default:
                button = MouseButton.Left; break;
        }
    }
}

public class MouseClickUncaught : MouseClickEvent
{
    public MouseClickUncaught(InputAction.CallbackContext ctx) : base(ctx) { }
}

public class PlayerHotkey : BaseInputEvent
{
    public int number = -1;

    public PlayerHotkey(InputAction.CallbackContext ctx) : base(ctx)
    {
        number = int.Parse(ctx.control.name);
    }
}

public class PlayerInteractEvent : BaseInputEvent
{
    public PlayerInteractEvent(InputAction.CallbackContext ctx) : base(ctx) { }
}

public class PlayerEscapeEvent : BaseInputEvent
{
    public PlayerEscapeEvent(InputAction.CallbackContext ctx) : base(ctx) { }
}
*/