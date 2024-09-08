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

    private void OnCountingTime(uint time)
    {
        float normalizedTime = time * 1.0f / _dayNightCycleService.MINUTES_IN_CYCLE;
        SetAnimationAtTime(normalizedTime);
    }

    private void SetAnimationAtTime(float normalizedTime)
    {
        _animator.Play(_animationClip.name, 0, normalizedTime);
        _animator.speed = 0;
    }

    private void OnDestroy()
    {
        _dayNightCycleService.Time -= OnCountingTime;
    }
}