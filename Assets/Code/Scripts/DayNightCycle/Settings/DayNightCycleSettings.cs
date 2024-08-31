using UnityEngine;

[CreateAssetMenu(fileName = "DayNightCycle", menuName = "Settings/DayNightCycle")]
public class DayNightCycleSettings : ScriptableObject
{
    public float lightIntensity;
    [Range(0, 23)] public byte startHour;
    [Range(0, 59)] public byte startMinute;
    public uint timeIncrementInMinutes = 1;
    public float timeSpeed = 1;

}