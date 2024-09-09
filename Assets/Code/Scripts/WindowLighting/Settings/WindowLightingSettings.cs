using UnityEngine;

[CreateAssetMenu(fileName = "WindowLighting", menuName = "Settings/WindowLighting")]
public class WindowLightingSettings : ScriptableObject
{
    public PartOfDay turnLightOn;
    public GameTimeData maxTurningOnDelay;
    public PartOfDay turnLightOff;
    public GameTimeData maxTurningOffDelay;

}