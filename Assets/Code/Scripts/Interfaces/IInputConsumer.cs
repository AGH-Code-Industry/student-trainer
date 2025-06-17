using UnityEngine;

public interface IInputConsumer
{
    /// <summary>
    /// Dictates the order in which the scripts will receive the input
    /// </summary>
    int priority { get; }

    /// <summary>
    /// Called by the input controller when an appropriate input has been received
    /// </summary>
    /// <returns>Returns true if the input has been "caught" or "consumed", and should not be propagated to other scripts.</returns>
    bool ConsumeInput(UnityEngine.InputSystem.InputAction.CallbackContext context);
    //aaaa
}
