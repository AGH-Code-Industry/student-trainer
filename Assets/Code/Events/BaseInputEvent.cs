using UnityEngine.InputSystem;

public class BaseInputEvent
{
    public InputAction.CallbackContext ctx;

    public BaseInputEvent(InputAction.CallbackContext ctx)
    {
        this.ctx = ctx;
    }

}