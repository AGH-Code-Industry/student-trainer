using Chapter.Singleton;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    CustomActions input;
    InventoryManager inventoryManager;
    public CustomActions GetInput() => input ??= CreateInput();

    private CustomActions CreateInput()
    {
        input = new CustomActions();
        input.Enable();
        return input;
    }

    private void OnDestroy()
    {
        input?.Dispose();
    }
    private void Start()
    {
        inventoryManager = new InventoryManager();
        inventoryManager.OnStart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryManager.ChangeState();
        }
    }
}