using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Animator _animator;
    private float _actualTimeInMinutes;
    private uint _targetTimeInMinutes;

    private float _timer, _timeToNewSunPosition;

    private AnimationClip _animationClip;

    [Inject]
    private DayNightCycleService _dayNightCycleService;


    private void Awake()
    {
        _dayNightCycleService.Time += OnCountingTime;
        _animationClip = _animator.runtimeAnimatorController.animationClips.First();
    }

    private void Start()
    {
        _timeToNewSunPosition = 2;
        _actualTimeInMinutes = _dayNightCycleService.Settings.startTime.ToMinutes();
        _targetTimeInMinutes = (uint)_actualTimeInMinutes + _dayNightCycleService.Settings.timeIncrementInMinutes;
    }

    private void Update()
    {
        if (_actualTimeInMinutes == _targetTimeInMinutes)
        {
            _targetTimeInMinutes = (uint)_actualTimeInMinutes + _dayNightCycleService.Settings.timeIncrementInMinutes;
            _dayNightCycleService.SetTime((uint)_actualTimeInMinutes);
        }

        _actualTimeInMinutes = Mathf.MoveTowards(_actualTimeInMinutes, _targetTimeInMinutes, Time.deltaTime * _dayNightCycleService.Settings.timeSpeed);

        float normalizedAnimationAtTime = _actualTimeInMinutes * 1.0f / _dayNightCycleService.MINUTES_IN_CYCLE;
        SetAnimationAtTime(normalizedAnimationAtTime);
    }

    private void OnCountingTime(GameTimeData time)
    {
        float normalizedAnimationAtTime = time.ToMinutes() * 1.0f / _dayNightCycleService.MINUTES_IN_CYCLE;
        SetAnimationAtTime(normalizedAnimationAtTime);
    }

    private void SetAnimationAtTime(float normalizedAnimationAtTime)
    {
        _animator.Play(_animationClip.name, 0, normalizedAnimationAtTime);
        _animator.speed = 0;
    }

    private void OnDestroy()
    {
        _dayNightCycleService.Time -= OnCountingTime;
    }
}