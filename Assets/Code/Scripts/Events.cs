using System;
using UnityEngine;

public static class Events
{
    public static Action<TextAsset> DialogTrigger; 
    public static event Action OnScooterSpawnRequested; // Event for scooter spawning
    public static void TriggerScooterSpawn()
    {
        OnScooterSpawnRequested?.Invoke();
    }
}