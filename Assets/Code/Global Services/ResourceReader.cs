using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReader
{
    private Dictionary<Type, string> path = new()
    {
        { typeof(DayNightCycleSettings), "DayNightCycle" },
    };

    public T ReadSettings<T>() where T : ScriptableObject
        => Resources.Load<T>(path[typeof(T)]);
}