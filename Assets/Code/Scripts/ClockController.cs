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
        string h = time.hour.ToString();
        string m = time.minute.ToString();
        text.text = h + ':' + m.PadLeft(2, '0');
    }

    private void OnDestroy()
    {
        dayNightCycleService.Time -= OnChangeTime;
    }
}