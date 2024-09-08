using UnityEngine;

[CreateAssetMenu(fileName = "Windows", menuName = "Settings/Windows")]
public class WindowSettings : ScriptableObject
{
    public PartOfDay turnLightOn;
    public GameTimeData maxTurningOnDelay;
    public PartOfDay turnLightOff;
    public GameTimeData maxTurningOffDelay;

}