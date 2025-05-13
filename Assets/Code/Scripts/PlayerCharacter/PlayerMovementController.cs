using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    CharacterController _controller;
    Vector3 _currentVector = Vector3.zero;

    [Inject] readonly PlayerService _service;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _service.PlayerPosition = transform.position;
        _service.CheckIfGrounded();

        _currentVector = _service.GetMovementVector();
        if (_currentVector != Vector3.zero)
        {
            Vector3 rotVector = _currentVector;
            rotVector.y = 0;
            if(rotVector != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(rotVector);

            _controller.Move(_currentVector * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {

    }
}