using System;
using Zenject;
using System.Threading;


public class DayNightCycleService : IInitializable, IDisposable
{
    public readonly uint MINUTES_IN_CYCLE = 1440;

    public event Action<uint> Time;
    private uint _actualTime = 0;
    private DayNightCycleSettings _settings;
    private CancellationTokenSource _cancellationToken;

    [Inject]
    private ResourceReader _resourceReader;

    public void Initialize()
    {
        _settings = _resourceReader.ReadSettings<DayNightCycleSettings>();
        SetTime(_settings.startHour, _settings.startMinute);
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

    private void SetTime(byte hour, byte minute) => SetTime((uint)(hour * 24 + minute));

    private void SetTime(uint minutes)
    {
        _actualTime = minutes;
        if (_actualTime >= MINUTES_IN_CYCLE)
            _actualTime %= MINUTES_IN_CYCLE;
        Time?.Invoke(_actualTime);
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