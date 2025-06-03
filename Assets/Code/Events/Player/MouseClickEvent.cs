using UnityEngine;
using UnityEngine.InputSystem;

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

        switch(ctx.control.name)
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