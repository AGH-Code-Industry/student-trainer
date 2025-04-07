using UnityEngine;

// Defines an object that the player can interact with: doors to open, item to pick up, texts with lore to read, etc.
// Scripts implementing this interface should always be present at the ROOT of the interactable object
public interface IInteractable
{
    public void Interact();
    public string GetObjectName();
    // Co się robi z obiektem, np: [E] podnieś, [E] otwórz
    public string GetActionName();
    public Transform GetTransform();

    public bool InteractionAllowed();
    //public string NoInteractionText();
}
