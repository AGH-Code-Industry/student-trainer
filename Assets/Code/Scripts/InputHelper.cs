using UnityEngine;
using UnityEngine.InputSystem;

public static class InputHelper
{
    public class MouseClickData
    {
        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }
        public MouseButton button;
        public bool shiftModifier = false;

        public MouseClickData(InputAction.CallbackContext ctx)
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

    public static int ParseHotkey(InputAction.CallbackContext ctx)
    {
        return int.Parse(ctx.control.name);
    }
}