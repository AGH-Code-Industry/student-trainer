using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    CharacterController _controller;
    Vector3 _currentVector = Vector3.zero;

    [Inject] readonly PlayerMovementService _service;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _currentVector = _service.GetMovementVector();
        if (_currentVector != Vector3.zero)
        {
            _controller.Move(_currentVector * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        
    }
}