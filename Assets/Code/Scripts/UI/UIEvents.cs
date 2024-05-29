using System;

public static class UIEvents
{
    public static Action CloseDialogue; 
    public static Action<int> SelectedDialogueChoice;
    public static Action NextDialogue;

    public static Action ClickPlay;
    public static Action ClickSettings;
    public static Action ClickQuit;
    public static Action ClickBack;
}