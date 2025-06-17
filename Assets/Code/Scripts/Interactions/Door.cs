using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] Animator anim;
    [SerializeField] Transform gfxTransform;

    [Tooltip("Leave empty if none is required")]
    [SerializeField] ItemPreset itemRequiredToOpen = null;
    [SerializeField] bool dontRemoveAfterUse = false;

    [SerializeField] RoomRevealer roomFogController;
    public UnityEvent DoorOpening, DoorClosing;

    [Inject] readonly InventoryService invService;

    bool isUnlocked = true;
    bool playerHasReqItem = false;

    bool interactionFocused = false;

    public event System.Action onObjectChanged, onInteractionDestroyed;


    enum DoorState
    {
        Open,
        Closed,
        Moving
    };

    DoorState state = DoorState.Closed;

    // Start is called before the first frame update
    void Start()
    {
        if (itemRequiredToOpen != null)
            isUnlocked = false;

        FindAnyObjectByType<PlayerInteractions>().RegisterInteractable(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionFocused)
        {
            bool hasItem = invService.HasItem(itemRequiredToOpen);
            if (hasItem != playerHasReqItem)
            {
                playerHasReqItem = hasItem;
                onObjectChanged?.Invoke();
            }
        }
    }



    public void Interact()
    {
        if (!isUnlocked)
        {
            if (playerHasReqItem)
            {
                if (!dontRemoveAfterUse)
                    invService.RemoveItem(itemRequiredToOpen, 1);

                isUnlocked = true;
            }
            return;
        }

        if (state == DoorState.Moving)
            return;

        if (state == DoorState.Closed)
            StartCoroutine(OpenCoroutine());
        else
            StartCoroutine(CloseCoroutine());
    }

    public GameObject GetGO() => gameObject;

    public InteractableData GetInteractionData()
    {
        string objName = isUnlocked ? "Drzwi" : "Zamknięte Drzwi";
        string actionName = "";

        if(isUnlocked)
            actionName = state == DoorState.Closed ? "otwórz" : "zamknij";
        else
            actionName = playerHasReqItem ? "odblokuj" : $"wymagane \"{itemRequiredToOpen.name}\"";

        return new InteractableData(objName, actionName, true, isUnlocked || playerHasReqItem);
    }

    public void FocusInteraction(bool isFocused) { interactionFocused = isFocused; }



    public void OnDoorOpening()
    {
        DoorOpening?.Invoke();
    }

    public void OnDoorClosing()
    {
        DoorClosing?.Invoke();
    }

    

    

    public string GetActionName()
    {
        if (isUnlocked)
        {
            if (state == DoorState.Closed)
                return "otwórz";
            else
                return "zamknij";
        }
        else
        {
            if (playerHasReqItem)
                return "odblokuj";
            else
                return "wymagane \"" + itemRequiredToOpen.name + "\"";
        }
    }

    public bool InteractionAllowed()
    {
        return isUnlocked || playerHasReqItem;
    }

    IEnumerator OpenCoroutine()
    {
        if (roomFogController)
            roomFogController.Reveal();

        state = DoorState.Moving;
        anim.SetTrigger("open");

        yield return new WaitForSeconds(1f);

        state = DoorState.Open;
        onObjectChanged?.Invoke();
    }

    IEnumerator CloseCoroutine()
    {
        if (roomFogController)
            roomFogController.Hide();

        state = DoorState.Moving;
        anim.SetTrigger("close");

        yield return new WaitForSeconds(1f);

        state = DoorState.Closed;
        onObjectChanged?.Invoke();
    }

    private void OnDestroy()
    {
        FindAnyObjectByType<PlayerInteractions>()?.RemoveInteractable(this);
    }
}
