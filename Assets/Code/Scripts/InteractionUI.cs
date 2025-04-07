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

        module.onInteractionPossible += UpdateInteractable;
        module.onInteractionLost += HidePrompt;
    }

    void FixedUpdate()
    {
        if(promptShown)
        {
            ShowPrompt();
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

    public void UpdateInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;

        if (currentInteractable == null)
            HidePrompt();
        else
            ShowPrompt();
    }

    public void ShowPrompt()
    {
        promptObject.SetActive(true);

        string objName = currentInteractable.GetObjectName();
        string actionName = currentInteractable.GetActionName();
        bool allowed = currentInteractable.InteractionAllowed();

        string toShow = objName + ": ";

        if (allowed)
            toShow += "[" + interactionKey + "] ";

        toShow += actionName;

        promptText.text = toShow;
        promptShown = true;

        currentInterTransform = currentInteractable.GetTransform();
        PositionPrompt();
    }

    public void HidePrompt()
    {
        promptObject.SetActive(false);
        currentInteractable = null;
        currentInterTransform = null;
        promptShown = false;
    }



    private void OnDestroy()
    {
        module.onInteractionPossible -= UpdateInteractable;
        module.onInteractionLost -= HidePrompt;
    }
}