using UnityEngine;

[CreateAssetMenu(fileName = "StreetLampLighting", menuName = "Settings/StreetLampLighting")]
public class StreetLampLightingSettings : ScriptableObject
{
    public PartOfDay turnLightOn;
    public GameTimeData maxTurningOnDelay;
    public PartOfDay turnLightOff;
    public GameTimeData maxTurningOffDelay;

}