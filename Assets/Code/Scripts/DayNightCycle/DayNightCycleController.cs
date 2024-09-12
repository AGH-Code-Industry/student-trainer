using System.Linq;
using UnityEngine;
using Zenject;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Animator _animator;

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
        _dayNightCycleService.Start();
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