using Chapter.Singleton;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    private Animator animator;
    CustomActions input;
    public CustomActions GetInput() => input;
    public InventoryManager GetInventoryManager() => InventoryManager.Instance;

    private void Awake()
    {
        CreateInput();
    }

    public UnityEvent onScooterSpawnRequested = new UnityEvent();

    private CustomActions CreateInput()
    {
        input = new CustomActions();
        input.Disable();
        return input;
    }

    private void OnDestroy()
    {
        input?.Dispose();
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