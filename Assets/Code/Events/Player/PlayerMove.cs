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