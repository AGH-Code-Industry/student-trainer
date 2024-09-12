using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClockController : MonoBehaviour
{
    private TextMeshProUGUI text;

    [Inject] private DayNightCycleService dayNightCycleService;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        dayNightCycleService.Time += OnChangeTime;
    }

    private void OnChangeTime(GameTimeData time)
    {
        text.text = $"{time.hour}:{time.minute}";
    }

    private void OnDestroy()
    {
        dayNightCycleService.Time -= OnChangeTime;
    }
}