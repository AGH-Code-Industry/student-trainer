using UnityEngine;
using Zenject;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] Transform _visibleModel;

    Vector3 _currentMovementVector;
    Quaternion _currentModelRotation;

    float _currentX = 0, _currentY = 0;
    [SerializeField] float _interpolationSpeed = 5f;

    [Inject] readonly PlayerMovementService _movement;

    void Start()
    {
        
    }

    void Update()
    {
        Animate();

        if (!_movement.frozen)
            FaceMouse();
    }

    Vector2 GetAnimationVector(Vector3 movement, float rotation)
    {
        float theta = Mathf.Deg2Rad * rotation;

        float x = movement.x * Mathf.Cos(theta) - movement.z * Mathf.Sin(theta);
        float y = movement.x * Mathf.Sin(theta) + movement.z * Mathf.Cos(theta);

        _currentX = Mathf.MoveTowards(_currentX, x, _interpolationSpeed * Time.deltaTime);
        _currentY = Mathf.MoveTowards(_currentY, y, _interpolationSpeed * Time.deltaTime);

        return new Vector2(_currentX, _currentY);
    }

    void Animate()
    {
        _currentMovementVector = _movement.GetMovementVector();
        float angle = _currentModelRotation.eulerAngles.y;
        Vector2 animVector = GetAnimationVector(_currentMovementVector, angle);

        _animator.SetFloat("Move_X", animVector.x);
        _animator.SetFloat("Move_Y", animVector.y);
    }

    void FaceMouse()
    {
        Vector3 direction = _movement.GetLookVector() - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction, _visibleModel.up);
        Vector3 euler = lookRot.eulerAngles;
        euler.x = 0; euler.z = 0;
        _currentModelRotation = Quaternion.Euler(euler);

        _visibleModel.rotation = Quaternion.RotateTowards(_visibleModel.rotation, _currentModelRotation, _movement.GetRotationSpeed() * Time.deltaTime);
        //_visibleModel.eulerAngles = euler;
    }
}
