using System;
using Zenject;
using System.Threading;
using System.Diagnostics;


public class DayNightCycleService : IInitializable, IDisposable
{
    public readonly uint MINUTES_IN_CYCLE = 1440;

    public event Action<GameTimeData> Time;
    public event Action<PartOfDay> PartOfDay;

    private GameTimeData _actualTime = new GameTimeData();
    public DayNightCycleSettings Settings => _settings;
    private DayNightCycleSettings _settings;

    private PartOfDaySettings _partOfDaySettings;
    private CancellationTokenSource _cancellationToken;

    [Inject] private ResourceReader _resourceReader;

    public void Initialize()
    {
        _settings = _resourceReader.ReadSettings<DayNightCycleSettings>();
        _partOfDaySettings = _resourceReader.ReadSettings<PartOfDaySettings>();
        SetTime(_settings.startTime);
    }

    public void Start()
    {
        if (_cancellationToken != null)
        {
            return;
        }

        _cancellationToken = new CancellationTokenSource();
    }

    public void Stop()
    {
        _cancellationToken?.Cancel();
        _cancellationToken = null;
    }

    public void SetTime(GameTimeData time) => SetTime((uint)(time.hour * 60 + time.minute));

    public void SetTime(uint minutes)
    {
        if (minutes >= MINUTES_IN_CYCLE)
            minutes %= MINUTES_IN_CYCLE;

        _actualTime.SetTime(minutes);
        Time?.Invoke(_actualTime);
        CheckTimeOfDay(minutes);
    }

    private void CheckTimeOfDay(uint minutes)
    {
        foreach (var partOfDay in _partOfDaySettings.data)
        {
            if (partOfDay.time.ToMinutes() == minutes)
            {
                PartOfDay?.Invoke(partOfDay.partOfDay);
                break;
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}