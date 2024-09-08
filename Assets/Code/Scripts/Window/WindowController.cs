using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class WindowController : MonoBehaviour
{
    private Renderer objectRenderer;
    private MaterialPropertyBlock propertyBlock;
    private WindowSettings _windowSettings;
    private DayNightCycleSettings _dayNightCycleSettings;


    [Inject]
    private DayNightCycleService _dayNightCycleService;
    [Inject]
    private ResourceReader _resourceReader;

    private void Awake()
    {
        _dayNightCycleService.PartOfDay += OnTimeOfDay;
        _windowSettings = _resourceReader.ReadSettings<WindowSettings>();
        _dayNightCycleSettings = _resourceReader.ReadSettings<DayNightCycleSettings>();
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnTimeOfDay(PartOfDay partOfDay)
    {
        if (_windowSettings.turnLightOff == partOfDay)
        {
            StartCoroutine(TurnOffLight());
        }

        if (_windowSettings.turnLightOn == partOfDay)
        {
            if (Random.Range(0, 2) == 1)
                StartCoroutine(TurnOnLight());
        }
    }

    IEnumerator TurnOffLight()
    {
        objectRenderer.GetPropertyBlock(propertyBlock);

        var milliseconds = _windowSettings.maxTurningOffDelay.ToMinutes() * _dayNightCycleSettings.timeSpeed;

        yield return new WaitForSeconds(Random.Range(0, milliseconds) / 1000.0f);

        propertyBlock.SetInteger("_IsLightOn", 0);

        objectRenderer.SetPropertyBlock(propertyBlock);

        yield return null;
    }

    IEnumerator TurnOnLight()
    {
        objectRenderer.GetPropertyBlock(propertyBlock);

        var milliseconds = _windowSettings.maxTurningOnDelay.ToMinutes() * _dayNightCycleSettings.timeSpeed;

        yield return new WaitForSeconds(Random.Range(0, milliseconds) / 1000.0f);

        propertyBlock.SetInteger("_IsLightOn", 1);

        objectRenderer.SetPropertyBlock(propertyBlock);

        yield return null;
    }

    private void OnDestroy()
    {
        _dayNightCycleService.PartOfDay -= OnTimeOfDay;
    }
}