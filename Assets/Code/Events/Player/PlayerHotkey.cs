using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHotkey : BaseInputEvent
{
    public int number = -1;

    public PlayerHotkey(InputAction.CallbackContext ctx) : base(ctx)
    {
        number = int.Parse(ctx.control.name);
    }
}