using UnityEngine;
using Zenject;

[RequireComponent(typeof(Light))]
public class StreetLampLighting : MonoBehaviour
{
    private Light _light;
    private StreetLampLightingSettings _streetLampSettings;

    [Inject]
    private DayNightCycleService _dayNightCycleService;
    [Inject]
    private ResourceReader _resourceReader;

    private void Awake()
    {
        _dayNightCycleService.PartOfDay += OnTimeOfDay;
        _streetLampSettings = _resourceReader.ReadSettings<StreetLampLightingSettings>();
    }

    void Start()
    {
        _light = GetComponent<Light>();
    }

    private void OnTimeOfDay(PartOfDay partOfDay)
    {
        if (_streetLampSettings.turnLightOff == partOfDay)
        {
            _light.enabled = false;
        }

        if (_streetLampSettings.turnLightOn == partOfDay)
        {
            _light.enabled = true;
        }
    }

    private void OnDestroy()
    {
        _dayNightCycleService.PartOfDay -= OnTimeOfDay;
    }
}