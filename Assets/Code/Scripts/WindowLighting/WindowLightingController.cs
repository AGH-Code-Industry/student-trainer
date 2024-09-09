using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class WindowLightingController : MonoBehaviour
{
    private Renderer _objectRenderer;
    private MaterialPropertyBlock _propertyBlock;
    private WindowLightingSettings _windowSettings;
    private DayNightCycleSettings _dayNightCycleSettings;


    [Inject]
    private DayNightCycleService _dayNightCycleService;
    [Inject]
    private ResourceReader _resourceReader;

    private void Awake()
    {
        _dayNightCycleService.PartOfDay += OnTimeOfDay;
        _windowSettings = _resourceReader.ReadSettings<WindowLightingSettings>();
        _dayNightCycleSettings = _resourceReader.ReadSettings<DayNightCycleSettings>();
    }

    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void OnTimeOfDay(PartOfDay partOfDay)
    {
        if (_windowSettings.turnLightOff == partOfDay)
        {
            StopAllCoroutines();
            StartCoroutine(TurnOffLight());
        }

        if (_windowSettings.turnLightOn == partOfDay)
        {
            if (Random.Range(0, 2) == 1)
            {
                StopAllCoroutines();
                StartCoroutine(TurnOnLight());
            }
        }
    }

    IEnumerator TurnOffLight()
    {
        _objectRenderer.GetPropertyBlock(_propertyBlock);

        var milliseconds = _windowSettings.maxTurningOffDelay.ToMinutes() * _dayNightCycleSettings.timeSpeed;

        yield return new WaitForSeconds(Random.Range(0, milliseconds) / 1000.0f);

        _propertyBlock.SetInteger("_IsLightOn", 0);

        _objectRenderer.SetPropertyBlock(_propertyBlock);

        yield return null;
    }

    IEnumerator TurnOnLight()
    {
        _objectRenderer.GetPropertyBlock(_propertyBlock);

        var milliseconds = _windowSettings.maxTurningOnDelay.ToMinutes() * _dayNightCycleSettings.timeSpeed;

        yield return new WaitForSeconds(Random.Range(0, milliseconds) / 1000.0f);

        _propertyBlock.SetInteger("_IsLightOn", 1);

        _objectRenderer.SetPropertyBlock(_propertyBlock);

        yield return null;
    }

    private void OnDestroy()
    {
        _dayNightCycleService.PartOfDay -= OnTimeOfDay;
    }
}