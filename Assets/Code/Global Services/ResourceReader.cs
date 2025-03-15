using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceReader
{
    private Dictionary<Type, string> path = new()
    {
        { typeof(DayNightCycleSettings), "DayNightCycle" },
        { typeof(PartOfDaySettings), "PartOfDay" },
        { typeof(WindowLightingSettings), "WindowLighting" },
        { typeof(StreetLampLightingSettings), "StreetLampLighting" },
        { typeof(CinemachineCameraSettings), "CinemachineCamera" },
        { typeof(PlayerMovementSettings), "PlayerMovement" },
        { typeof(ComboList), "PlayerComboList"}
    };

    public IEnumerable<T> ReadAllSettings<T>() where T : ScriptableObject
    {
        var settings = Resources.LoadAll<T>(path[typeof(T)]);
        if (settings == null)
            throw new FileLoadException($"The {typeof(T)} setting named {path[typeof(T)]} could not be found in the Resource folder.");
        return settings;
    }

    public T ReadSettings<T>() where T : ScriptableObject
    {
        var settings = Resources.Load<T>(path[typeof(T)]);
        if (settings == null)
            throw new FileLoadException($"The {typeof(T)} setting named {path[typeof(T)]} could not be found in the Resource folder.");
        return settings;
    }

    public CharacterSettings ReadCharacterSettings(CharacterType type)
    {
        var path = "Characters/" + type.ToString();
        var settings = Resources.Load<CharacterSettings>(path);
        if (settings == null)
            throw new FileLoadException($"The {typeof(CharacterSettings)} setting named {path} could not be found in the Resource folder.");
        return settings;
    }

}