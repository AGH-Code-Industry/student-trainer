using UnityEngine;
using Zenject;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] private Light _light;

    [Inject]
    private DayNightCycleService _dayNightCycleService;


    private void Awake()
    {
        _dayNightCycleService.Time += OnCountingTime;
    }

    private void Start()
    {
        _dayNightCycleService.Start();
    }

    private void OnCountingTime(uint time)
    {
        SetRotation(time);
        SetLightIntensity(time);
    }

    private void SetRotation(uint time) => transform.localRotation = Quaternion.Euler(0.25f * time - 90, 0, 0);

    private void SetLightIntensity(uint time)
    {
        float x = time * 1.0f / _dayNightCycleService.MINUTES_IN_CYCLE;
        var intensity = Mathf.Sin((2 * Mathf.PI * x) - (Mathf.PI / 2));
        _light.intensity = intensity < 0 ? 0 : intensity;
    }

    private void OnDestroy()
    {
        _dayNightCycleService.Time -= OnCountingTime;
    }
}