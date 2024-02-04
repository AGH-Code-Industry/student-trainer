using Chapter.Singleton;
using UnityEngine;

public class InputManager : Singleton<InputManager> 
{
    CustomActions input;

    public CustomActions GetInput() => input ??= CreateInput();

    private CustomActions CreateInput()
    {
        input = new CustomActions();
        input.Enable();
        return input;
    }

}