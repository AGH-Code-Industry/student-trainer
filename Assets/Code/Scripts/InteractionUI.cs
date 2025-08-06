using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    PlayerInteractions module;

    [SerializeField] GameObject promptObject;
    // Update to take the key from PlayerInput?
    [SerializeField] string interactionKey = "E";

    [SerializeField] Vector2 promptOffset;

    TextMeshProUGUI promptText;
    RectTransform promptTransform;

    Transform currentInterTransform;
    bool promptShown = false;

    Camera mainCamera;

    IInteractable currentInteractable;

    void Start()
    {
        module = FindAnyObjectByType<PlayerInteractions>();

        promptText = promptObject.GetComponent<TextMeshProUGUI>();
        promptTransform = promptObject.GetComponent<RectTransform>();

        mainCamera = Camera.main;

        HidePrompt();

        module.onInteractionPossible += ShowPrompt;
        module.onInteractionLost += HidePrompt;
    }

    void FixedUpdate()
    {
        if(promptShown)
        {
            PositionPrompt();
        }
    }

    void PositionPrompt()
    {
        if (!currentInterTransform)
            return;

        Vector2 screenPos = mainCamera.WorldToScreenPoint(currentInterTransform.position);

        screenPos += promptOffset;
        promptTransform.anchoredPosition = screenPos;
    }

    public void ShowPrompt(IInteractable interactable)
    {
        currentInteractable = interactable;
        currentInteractable.onObjectChanged += UpdatePrompt;

        promptObject.SetActive(true);
        
        promptShown = true;

        currentInterTransform = currentInteractable.GetGO().transform;
        UpdatePrompt();
        PositionPrompt();
    }

    public void UpdatePrompt()
    {
        if (currentInteractable == null)
            return;

        InteractableData data = currentInteractable.GetInteractionData();

        string toShow = data.objectName + ": ";

        if (data.interactionAllowed)
            toShow += "[" + interactionKey + "] ";

        toShow += data.actionName;

        promptText.text = toShow;
    }

    public void HidePrompt()
    {
        if(currentInteractable != null)
            currentInteractable.onObjectChanged -= UpdatePrompt;

        currentInteractable = null;
        currentInterTransform = null;

        promptObject.SetActive(false);
        promptShown = false;
    }



    private void OnDestroy()
    {
        if (currentInteractable != null)
            currentInteractable.onObjectChanged -= UpdatePrompt;

        module.onInteractionPossible -= ShowPrompt;
        module.onInteractionLost -= HidePrompt;
    }
}