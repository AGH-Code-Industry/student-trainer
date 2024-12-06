using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    //enum PlayerAnimation { Idle, Run }
    const string ANIM_IDLE = "Idle", ANIM_RUN = "Run";

    CharacterController _controller;
    [SerializeField] Animator _animator;
    Vector3 _currentVector = Vector3.zero;

    [SerializeField] Transform _visibleModel;

    [Inject] PlayerMovementService _service;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_currentVector != Vector3.zero)
        {
            _controller.Move(_currentVector * Time.deltaTime);
            FaceDirection();
        }
    }

    public void OnMove(InputValue val)
    {
        Vector2 input = val.Get<Vector2>();
        _currentVector = _service.GetMovementVector(input);

        PlayAnimation(input != Vector2.zero);
    }

    void PlayAnimation(bool _startedMoving)
    {
        if(!_startedMoving)
        {
            _animator.Play(ANIM_IDLE);
        }
        else
        {
            _animator.Play(ANIM_RUN);
        }
    }

    void FaceDirection()
    {
        Vector3 norm = _currentVector.normalized;
        Quaternion lookRot = Quaternion.LookRotation(norm, _visibleModel.up);
        _visibleModel.rotation = Quaternion.RotateTowards(_visibleModel.rotation, lookRot, _service.GetRotationSpeed() * Time.deltaTime);
    }

    private void OnDestroy()
    {
        
    }
}