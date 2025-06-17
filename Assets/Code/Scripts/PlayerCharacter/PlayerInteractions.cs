using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class PlayerInteractions : MonoBehaviour, IInputConsumer
{
    [SerializeField] float interactionRadius = 3f;

    [Inject] readonly EventBus eventBus;

    IInteractable currentInteractable = null;

    // Called when a new interactable object is in range or when the closest interactable object changes
    public Action<IInteractable> onInteractionPossible;
    public Action onInteract;
    // Called when there are no possible interactions
    public Action onInteractionLost;

    PlayerAnimationController animController;

    [Inject] readonly PlayerService playerService;
    [Inject] readonly InputService inputService;

    public readonly SystemFreezer freezer = new SystemFreezer();

    struct RegisteredInteractable
    {
        public IInteractable interactionComponent;
        public GameObject gameObj;
        public Transform interactionTransform;

        public RegisteredInteractable(IInteractable _interactionComponent)
        {
            interactionComponent = _interactionComponent;
            gameObj = _interactionComponent.GetGO();
            interactionTransform = gameObj.transform;
        }
    }

    List<RegisteredInteractable> registered = new List<RegisteredInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<PlayerAnimationController>();

        //eventBus.Subscribe<PlayerInteractEvent>(OnInteract);

        List<string> wantedActions = new List<string>();
        wantedActions.Add("Interact");
        inputService.RegisterConsumer(this, wantedActions);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!freezer.Frozen)
        {
            IInteractable possibleInteractable = GetPossibleInteraction();
            AssignCurrent(possibleInteractable);
        }
    }

    bool IsRegistered(GameObject obj)
    {
        foreach(RegisteredInteractable reg in registered)
        {
            if (reg.gameObj == obj)
                return true;
        }

        return false;
    }

    public void RegisterInteractable(IInteractable toRegister)
    {
        GameObject registerObj = toRegister.GetGO();
        if(IsRegistered(registerObj))
        {
            Debug.LogError($"PlayerInteraction, RegisterInteractable(): object \"{registerObj.name}\" tried to register twice!");
            return;
        }

        registered.Add(new RegisteredInteractable(toRegister));
    }

    public void RemoveInteractable(IInteractable componentToRemove)
    {
        RegisteredInteractable? toRemove = null;
        foreach(RegisteredInteractable reg in registered)
        {
            if(reg.interactionComponent == componentToRemove)
            {
                toRemove = reg;
                break;
            }
        }

        if(toRemove == null || !toRemove.HasValue)
        {
            Debug.LogError($"PlayerInteraction, RegisterInteractable(): object \"{toRemove.Value.gameObj.name}\" tried to unregister, even though it wasn't registered!");
            return;
        }

        registered.Remove(toRemove.Value);
    }

    void AssignCurrent(IInteractable interactable)
    {
        if (interactable == null && currentInteractable != null)
        {
            currentInteractable.FocusInteraction(false);
            currentInteractable = null;
            onInteractionLost?.Invoke();
        }
        else if (currentInteractable != interactable)
        {
            currentInteractable = interactable;
            currentInteractable.FocusInteraction(true);
            onInteractionPossible?.Invoke(interactable);
        }
    }

    IInteractable GetPossibleInteraction()
    {
        if (registered.Count == 0)
            return null;
        else if (registered.Count == 1)
            return registered[0].interactionComponent;

        List<RegisteredInteractable> possibleInteractions = new List<RegisteredInteractable>();
        foreach(RegisteredInteractable reg in registered)
        {
            float dist = Vector3.Distance(transform.position, reg.interactionTransform.position);
            if (dist <= interactionRadius)
                possibleInteractions.Add(reg);
        }

        if (possibleInteractions.Count == 0)
            return null;
        else if (possibleInteractions.Count == 1)
            return possibleInteractions[0].interactionComponent;

        float closestDistance = Vector3.Distance(transform.position, possibleInteractions[0].interactionTransform.position);
        RegisteredInteractable closestInteractable = possibleInteractions[0];
        for (int i = 1; i < possibleInteractions.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, possibleInteractions[i].interactionTransform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestInteractable = possibleInteractions[i];
            }
        }

        return closestInteractable.interactionComponent;
    }
    /*
    void OnInteract(PlayerInteractEvent ev)
    {
        if (freezer.Frozen)
            return;

        if (!ev.ctx.performed)
            return;

        if (currentInteractable == null || !currentInteractable.GetInteractionData().interactionAllowed)
            return;

        Vector3 interPosition = currentInteractable.GetGO().transform.position;

        currentInteractable.Interact();

        if (currentInteractable.GetInteractionData().shouldPlayAnimation)
            animController.PlayInteractionAnim(interPosition);

        onInteract?.Invoke();
    }
    */
    public int priority { get; } = 1;

    public bool ConsumeInput(InputAction.CallbackContext context)
    {
        if (freezer.Frozen)
            return false;

        if (!context.performed)
            return false;

        if (currentInteractable == null || !currentInteractable.GetInteractionData().interactionAllowed)
            return false;

        Vector3 interPosition = currentInteractable.GetGO().transform.position;

        currentInteractable.Interact();

        if (currentInteractable.GetInteractionData().shouldPlayAnimation)
            animController.PlayInteractionAnim(interPosition);

        onInteract?.Invoke();

        return true;
    }

    private void OnDestroy()
    {
        //eventBus.Unsubscribe<PlayerInteractEvent>(OnInteract);
        inputService.RemoveConsumer(this);
    }
}