using System;
using Zenject;
using System.Threading;


public class DayNightCycleService : IInitializable, IDisposable
{
    public readonly uint MINUTES_IN_CYCLE = 1440;

    public event Action<uint> Time;
    public event Action<PartOfDay> TimeOfDay;

    private uint _actualTime = 0;
    private DayNightCycleSettings _settings;
    private PartOfDaySettings _partOfDaySettings;
    private CancellationTokenSource _cancellationToken;

    [Inject]
    private ResourceReader _resourceReader;

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
        StartCountingTimeAsync(_cancellationToken.Token);
    }

    public void Stop()
    {
        _cancellationToken?.Cancel();
        _cancellationToken = null;
    }

    private void SetTime(GameTimeData time) => SetTime((uint)(time.hour * 24 + time.minute));

    private void SetTime(uint minutes)
    {
        _actualTime = minutes;
        if (_actualTime >= MINUTES_IN_CYCLE)
            _actualTime %= MINUTES_IN_CYCLE;
        Time?.Invoke(_actualTime);

        CheckTimeOfDay(minutes);
    }

    private void CheckTimeOfDay(uint minutes)
    {
        foreach (var partOfDay in _partOfDaySettings.data)
        {
            if (partOfDay.time.ToMinutes() == minutes)
            {
                TimeOfDay?.Invoke(partOfDay.partOfDay);
                break;
            }
        }
    }

    private async void StartCountingTimeAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await System.Threading.Tasks.Task.Delay((int)(1000 / _settings.timeSpeed), token);
                SetTime(_actualTime + _settings.timeIncrementInMinutes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in StartCountingTimeAsync: {ex.Message}");
        }
    }

    public void Dispose()
    {
        Stop();
    }
}