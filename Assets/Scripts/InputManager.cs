using Chapter.Singleton;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    private Main2 rightclick;

    private Animator animator;
    CustomActions input;
    public CustomActions GetInput() => input;
    public Main2 GetInputMain2() => rightclick;

    public InventoryManager GetInventoryManager() => inventoryManager;

    private void Awake()
    {
        CreateInput();
        CreateMain2Input();
    }

    public UnityEvent onScooterSpawnRequested = new UnityEvent();

    private CustomActions CreateInput()
    {
        input = new CustomActions();
        input.Disable();
        return input;
    }

    private Main2 CreateMain2Input()
    {
        rightclick = new Main2();
        rightclick.Enable();
        return rightclick;
    }

    private void OnDestroy()
    {
        input?.Dispose();
        rightclick?.Dispose();
    }


    private void Start()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager.Instance.ChangeState();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            onScooterSpawnRequested.Invoke();
            if (animator == null) return;
            animator.SetBool("isRidingScooter", !animator.GetBool("isRidingScooter"));

        }
    }
}