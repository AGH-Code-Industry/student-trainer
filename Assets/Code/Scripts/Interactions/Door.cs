using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] Animator anim;
    [SerializeField] Transform gfxTransform;

    [Tooltip("Leave empty if none is required")]
    [SerializeField] ItemPreset itemRequiredToOpen = null;
    [SerializeField] bool dontRemoveAfterUse = false;

    [Inject] readonly InventoryService invService;

    bool isUnlocked = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
            // These calls to the inventory service can probably be reduced to only one, but we can think about it later
            if (invService.HasItem(itemRequiredToOpen))
                return "odblokuj";
            else
                return "wymagane \"" + itemRequiredToOpen.name + "\"";
        }
    }

    public string GetObjectName()
    {
        if (isUnlocked)
            return "Drzwi";
        else
            return "Zamknięte Drzwi";
    }

    public Transform GetTransform()
    {
        return gfxTransform;
    }

    public void Interact()
    {
        if(!isUnlocked)
        {
            if(invService.HasItem(itemRequiredToOpen))
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

    public bool InteractionAllowed()
    {
        return isUnlocked || invService.HasItem(itemRequiredToOpen);
    }

    IEnumerator OpenCoroutine()
    {
        state = DoorState.Moving;
        anim.SetTrigger("open");

        yield return new WaitForSeconds(1f);

        state = DoorState.Open;
    }

    IEnumerator CloseCoroutine()
    {
        state = DoorState.Moving;
        anim.SetTrigger("close");

        yield return new WaitForSeconds(1f);

        state = DoorState.Closed;
    }
}
