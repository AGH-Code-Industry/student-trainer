using UnityEngine;
using Zenject;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private Material _material;

    [Inject] readonly PlayerService _playerService;

    void Start()
    {
        _playerService.HealthChange += OnHealthChange;
    }

    private void OnHealthChange(float health)
    {
        var healthPercentage = health / _playerService.MaxHealth;
        var invert = 1 - healthPercentage;

        _material.SetFloat("_RemovedSegments", 1 + (invert * 4));
    }

    void OnDestroy()
    {
        _playerService.HealthChange -= OnHealthChange;
        _material.SetFloat("_RemovedSegments", 1);
    }
}