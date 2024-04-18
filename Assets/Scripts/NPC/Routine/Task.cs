using UnityEngine;
using System;

[System.Serializable]
public class Task
{
    public string taskName;
    public Daytime time;
    public Vector2 destination;
    public bool isCompleted;
}