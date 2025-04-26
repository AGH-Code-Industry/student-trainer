using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickUncaught : MouseClickEvent
{
    public MouseClickUncaught(InputAction.CallbackContext ctx) : base(ctx)
    {
        
    }
}