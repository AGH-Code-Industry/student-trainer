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
        module = FindObjectOfType<PlayerInteractions>();

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

        currentInterTransform = currentInteractable.GetTransform();
        UpdatePrompt();
        PositionPrompt();
    }

    public void UpdatePrompt()
    {
        if (currentInteractable == null)
            return;

        string objName = currentInteractable.GetObjectName();
        string actionName = currentInteractable.GetActionName();
        bool allowed = currentInteractable.InteractionAllowed();

        string toShow = objName + ": ";

        if (allowed)
            toShow += "[" + interactionKey + "] ";

        toShow += actionName;

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