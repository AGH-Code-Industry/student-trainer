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
    };

    public T ReadSettings<T>() where T : ScriptableObject
    {
        var settings = Resources.Load<T>(path[typeof(T)]);
        if (settings == null)
            throw new FileLoadException($"The {typeof(T)} setting named {path[typeof(T)]} could not be found in the Resource folder.");
        return settings;
    }

}