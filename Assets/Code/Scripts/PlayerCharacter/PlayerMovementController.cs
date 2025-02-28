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
        _service.PlayerPosition = transform.position;

        _currentVector = _service.GetMovementVector();
        if (_currentVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_currentVector);
            _controller.Move(_currentVector * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {

    }
}