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
        promptObject.SetActive(true);

        string objName = interactable.GetObjectName();
        string actionName = interactable.GetActionName();

        promptText.text = objName + ": [" + interactionKey + "] " + actionName;
        promptShown = true;

        currentInterTransform = interactable.GetTransform();
        PositionPrompt();
    }

    public void HidePrompt()
    {
        promptObject.SetActive(false);
        currentInterTransform = null;
        promptShown = false;
    }



    private void OnDestroy()
    {
        module.onInteractionPossible -= ShowPrompt;
        module.onInteractionLost -= HidePrompt;
    }
}