using UnityEngine;

/// <summary>
/// Defines an object that the player can interact with: doors to open, item to pick up, texts with lore to read, etc.
/// Each script implementing this interface should register itself with the PlayerInteractions.RegisterInteractable() when it's created,
/// and remove with PlayerInteractions.RegisterInteractable() when it's destroyed.
/// </summary>
public interface IInteractable
{
    void Interact();

    //bool IsEnabled();
    //Transform GetTransform();
    GameObject GetGO();
    InteractableData GetInteractionData();

    // Tells the interactable object if it currently is the "possible" interaction
    void FocusInteraction(bool isFocused);

    // Called when the inner state of the interactable object is changed, so that InteractionUI knows when to redraw
    event System.Action onObjectChanged, onInteractionDestroyed;
}

/// <summary>
/// Used to store usable data about the interactable object. The individual data may be changed at runtime.
/// </summary>
public struct InteractableData
{
    public string objectName;
    public string actionName;
    public bool shouldPlayAnimation;
    public bool interactionAllowed;

    public InteractableData(string _objName, string _actionName, bool _playAnim, bool _allowed)
    {
        objectName = _objName;
        actionName = _actionName;
        shouldPlayAnimation = _playAnim;
        interactionAllowed = _allowed;
    }
}
