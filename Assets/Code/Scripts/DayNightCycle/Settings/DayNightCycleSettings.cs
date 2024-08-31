using UnityEngine;

[CreateAssetMenu(fileName = "DayNightCycle", menuName = "Settings/DayNightCycle")]
public class DayNightCycleSettings : ScriptableObject
{
    public float lightIntensity;
    public GameTimeData startTime;
    public uint timeIncrementInMinutes = 1;
    public float timeSpeed = 1;
}