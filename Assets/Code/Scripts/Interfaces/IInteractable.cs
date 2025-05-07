using UnityEngine;

// Defines an object that the player can interact with: doors to open, item to pick up, texts with lore to read, etc.
// Scripts implementing this interface should always be present at the ROOT of the interactable object
public interface IInteractable
{
    public void Interact();
    public void EndInteraction();
    public string GetObjectName();
    // Co się robi z obiektem, np: [E] podnieś, [E] otwórz
    public string GetActionName();
    public Transform GetTransform();

    public bool InteractionAllowed();
    //public string NoInteractionText();

    // Tells the interactable object if it currently is the "possible" interaction
    public void FocusInteraction(bool isFocused);
    // Called when the inner state of the interactable object is changed, so that InteractionUI knows when to redraw
    public event System.Action onObjectChanged;

    public bool IsBlocking();
    public bool ShouldPlayAnimation();
}
