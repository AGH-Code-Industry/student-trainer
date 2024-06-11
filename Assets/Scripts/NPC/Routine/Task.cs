using UnityEngine;
using System;

[System.Serializable]
public class Task
{
    public string taskName;
    public Daytime startTime;
    public Daytime randomTimeDelay;
    public Vector3 destination;
}