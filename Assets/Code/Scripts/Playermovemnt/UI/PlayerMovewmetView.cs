using TMPro;
using UnityEngine;
using Zenject;

public class PlayerMovewmetView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [Inject] private PlayerMovementService _service;

    private void Start()
    {
        _service.Speed += OnSpeedChange;
    }

    private void OnSpeedChange(float speed)
    {
        _text.text = speed.ToString();
    }

    private void OnDestroy()
    {
        _service.Speed -= OnSpeedChange;
    }
}