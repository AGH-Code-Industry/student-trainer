using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class PlayerInteractions : MonoBehaviour
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
    bool duringBlockingInter = false;

    [Inject] readonly PlayerService playerService;

    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<PlayerAnimationController>();

        eventBus.Subscribe<PlayerInteractEvent>(OnInteract);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IInteractable possibleInteractable = GetPossibleInteraction();
        AssignCurrent(possibleInteractable);
    }

    void AssignCurrent(IInteractable interactable)
    {
        if(interactable == null && currentInteractable != null)
        {
            currentInteractable.FocusInteraction(false);
            currentInteractable = null;
            onInteractionLost?.Invoke();
        }
        else if(currentInteractable != interactable)
        {
            currentInteractable = interactable;
            currentInteractable.FocusInteraction(true);
            onInteractionPossible?.Invoke(interactable);
        }
    }

    IInteractable GetPossibleInteraction()
    {
        /*
         
        It's bad for performance to use GetComponent every frame, so if in the future it becomes a problem, it might be optimized this way:

        1. At map load get all interactables on the scene and store them in an array
        2. Create an event that signalizes a new interactable appearing on the map
        3. When the event fires, the interactable gets added to the list
        4. Loop over the list instead of using GetComponent
        5. Check the distance to every interactable and add it to possibleInteractions if it's smaller than the interactionRadius

        */


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);

        if(hitColliders.Length == 0)
        {
            return null;
        }

        List<GameObject> possibleInteractions = new List<GameObject>();
        foreach (Collider col in hitColliders)
        {
            GameObject rootGO = col.transform.root.gameObject;
            IInteractable interactionComponent = rootGO.GetComponent<IInteractable>();
            if (interactionComponent != null)
                if(interactionComponent.IsEnabled())
                    possibleInteractions.Add(rootGO);
        }

        if(possibleInteractions.Count == 0)
        {
            return null;
        }
        else if(possibleInteractions.Count == 1)
        {
            return possibleInteractions[0].GetComponent<IInteractable>();
        }

        float closestDistance = Vector3.Distance(transform.position, possibleInteractions[0].transform.position);
        GameObject closestInteractable = possibleInteractions[0];
        for(int i = 1; i < possibleInteractions.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, possibleInteractions[i].transform.position);
            if(dist < closestDistance)
            {
                closestDistance = dist;
                closestInteractable = possibleInteractions[i];
            }
        }

        return closestInteractable.GetComponent<IInteractable>();
    }

    void OnInteract(PlayerInteractEvent ev)
    {
        if (!ev.ctx.performed)
            return;

        if (currentInteractable == null)
            return;

        Vector3 interPosition = currentInteractable.GetTransform().position;

        if(currentInteractable.IsBlocking())
        {
            if(!duringBlockingInter)
            {
                currentInteractable.Interact();
                playerService.Freeze("interaction");
                duringBlockingInter = true;

                currentInteractable.onInteractionDestroyed += EndBlockingInteraction;

                if (currentInteractable.ShouldPlayAnimation())
                    animController.PlayInteractionAnim(interPosition);
            }
            else
            {
                EndBlockingInteraction();
            }
        }
        else
        {
            currentInteractable.Interact();

            if (currentInteractable.ShouldPlayAnimation())
                animController.PlayInteractionAnim(interPosition);
        }

        onInteract?.Invoke();
    }

    void EndBlockingInteraction()
    {
        currentInteractable.EndInteraction();
        playerService.Unfreeze("interaction");
        duringBlockingInter = false;

        currentInteractable.onInteractionDestroyed -= EndBlockingInteraction;
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<PlayerInteractEvent>(OnInteract);
    }
}
