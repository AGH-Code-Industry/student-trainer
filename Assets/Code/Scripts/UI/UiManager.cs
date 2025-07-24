using UnityEngine;
using System.Collections.Generic;
using Zenject;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour, IInputConsumer
{
    [SerializeField]
    UiWindowData[] availableWindows;
    List<string> openWindowIDs = new List<string>();

    [Inject] readonly PlayerService playerService;
    [Inject] readonly EventBus eventBus;
    [Inject] readonly InputService inputService;
    PlayerInteractions playerInteractions;
    PlayerCombat playerCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInteractions = FindAnyObjectByType<PlayerInteractions>();
        playerCombat = FindAnyObjectByType<PlayerCombat>();

        //eventBus.Subscribe<PlayerInteractEvent>(WindowCloseInter);
        //eventBus.Subscribe<PlayerEscapeEvent>(WindowCloseEscape);
        List<string> wantedActions = new List<string>();
        wantedActions.Add("Interact");
        wantedActions.Add("Escape");
        inputService.RegisterConsumer(this, wantedActions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UiWindowData GetDataFromID(string _id)
    {
        foreach(UiWindowData window in availableWindows)
        {
            if (window.id == _id)
                return window;
        }

        return null;
    }

    public bool CanOpenWindow(string _idToOpen)
    {
        if(openWindowIDs.Count == 0)
        {
            return true;
        }
        else
        {
            UiWindowData toOpen = GetDataFromID(_idToOpen);
            if (!toOpen.allowOtherWindows)
                return false;
        }

        foreach(string openWindow in openWindowIDs)
        {
            UiWindowData data = GetDataFromID(openWindow);
            if (!data.allowOtherWindows)
                return false;
        }

        return true;
    }

    public void OpenWindow(string _id)
    {
        UiWindowData data = GetDataFromID(_id);

        if(data == null)
        {
            Debug.LogError($"UiManager, OpenWindow(): window with passed id \"{_id}\" does not exist!");
            return;
        }

        // Window is already open or can't be currently opened
        if (openWindowIDs.Contains(data.id) || !CanOpenWindow(_id))
            return;

        if(data.lockPlayer)
        {
            playerService.freezer.Freeze(data.id);
            playerInteractions.freezer.Freeze(data.id);
            playerCombat.freezer.Freeze(data.id);
        }

        data.windowObject.SetActive(true);
        data.windowScript.OnWindowOpened();

        openWindowIDs.Add(_id);
    }

    public void CloseWindow(string _id)
    {
        UiWindowData data = GetDataFromID(_id);

        if (data == null)
        {
            Debug.LogError($"UiManager, OpenWindow(): window with passed id \"{_id}\" does not exist!");
            return;
        }

        // Window isn't open
        if (!openWindowIDs.Contains(data.id))
            return;

        if (data.lockPlayer)
        {
            playerService.freezer.Unfreeze(data.id);
            playerInteractions.freezer.Unfreeze(data.id);
            playerCombat.freezer.Unfreeze(data.id);
        }

        data.windowScript.OnWindowClosed();
        data.windowObject.SetActive(false);

        openWindowIDs.Remove(_id);
    }

    public void CloseAllWindows()
    {
        foreach(string openWindow in openWindowIDs)
        {
            CloseWindow(openWindow);
        }
    }
    /*
    void WindowCloseInter(PlayerInteractEvent ev)
    {
        if (!ev.ctx.performed)
            return;

        // Go backwards to avoid indexing issues
        for (int i = openWindowIDs.Count - 1; i >= 0; i--)
        {
            string id = openWindowIDs[i];
            UiWindowData window = GetDataFromID(id);

            bool canClose = window.closingMethod == WindowClosingMethod.InteractionKey || window.closingMethod == WindowClosingMethod.Both;
            if (canClose)
                CloseWindow(id);
        }
    }

    void WindowCloseEscape(PlayerEscapeEvent ev)
    {
        if (!ev.ctx.performed)
            return;

        // Go backwards to avoid indexing issues
        for (int i = openWindowIDs.Count - 1; i >= 0; i--)
        {
            string id = openWindowIDs[i];
            UiWindowData window = GetDataFromID(id);

            bool canClose = window.closingMethod == WindowClosingMethod.Escape || window.closingMethod == WindowClosingMethod.Both;
            if (canClose)
                CloseWindow(id);
        }
    }
    */
    public int priority { get; } = 1000;

    public bool ConsumeInput(InputAction.CallbackContext context)
    {
        if (!context.performed || openWindowIDs.Count == 0)
            return false;

        bool anyWindowClosed = false;
        for (int i = openWindowIDs.Count - 1; i >= 0; i--)
        {
            string id = openWindowIDs[i];
            UiWindowData window = GetDataFromID(id);

            if (context.action.name == "Interact")
            {
                bool canClose = window.closingMethod == WindowClosingMethod.InteractionKey || window.closingMethod == WindowClosingMethod.Both;
                if (canClose)
                {
                    anyWindowClosed = true;
                    CloseWindow(id);
                }
            }
            else if (context.action.name == "Escape")
            {
                bool canClose = window.closingMethod == WindowClosingMethod.Escape || window.closingMethod == WindowClosingMethod.Both;
                if (canClose)
                {
                    anyWindowClosed = true;
                    CloseWindow(id);
                }
            }
        }

        return anyWindowClosed;
    }

    private void OnDestroy()
    {
        //eventBus.Unsubscribe<PlayerInteractEvent>(WindowCloseInter);
        //eventBus.Unsubscribe<PlayerEscapeEvent>(WindowCloseEscape);
        inputService.RemoveConsumer(this);
    }
}

[System.Serializable]
public class UiWindowData
{
    public string id;
    public WindowUI windowScript;
    public GameObject windowObject;
    // Should the window lock player's movements and interactions?
    public bool lockPlayer;
    public WindowClosingMethod closingMethod;
    // Can other windows be displayed alongside this one?
    public bool allowOtherWindows;
}

public enum WindowClosingMethod
{
    Escape,
    InteractionKey,
    Both
}