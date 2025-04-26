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

    [Inject] readonly PlayerService _movement;
    [Inject] readonly EventBus _eventBus;
    [Inject] readonly InputService _inputService;

    void Start()
    {
        _eventBus.Subscribe<PlayerAttack>(OnAttack);
        _eventBus.Subscribe<PlayerDodge>(OnDodge);
    }

    void Update()
    {
        Animate();

        // if (!_movement.frozen)
        //     FaceMouse();
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

        var runOrWalk = _movement.IsRunning ? 1 : 0.5f;
        _animator.SetFloat("Move_X", animVector.magnitude > 0 ? runOrWalk : 0);
        //_animator.SetFloat("Move_Y", animVector.y);
    }

    private void OnDodge(PlayerDodge playerDodge)
    {
        if (playerDodge.ctx.started)
        {
            _animator.Play("Roll");
        }
    }

    private void OnAttack(PlayerAttack playerAttack)
    {
        if (playerAttack.ctx.started)
        {
            FaceMouse();
        }
    }

    void FaceMouse()
    {
        var lookVector = _inputService.MouseDownPosition;

        var direction = (lookVector - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);

        // Vector3 direction = lookVector - transform.position;
        // Quaternion lookRot = Quaternion.LookRotation(direction, _visibleModel.up);
        // Vector3 euler = lookRot.eulerAngles;
        // euler.x = 0; euler.z = 0;
        // _currentModelRotation = Quaternion.Euler(euler);

        // _visibleModel.rotation = _currentModelRotation;
        //_visibleModel.eulerAngles = euler;
    }

    public void PlayAnimation(string animName)
    {
        _animator.CrossFade(animName, 0.2f);
    }

    void OnDestroy()
    {
        _eventBus.Unsubscribe<PlayerAttack>(OnAttack);
        _eventBus.Unsubscribe<PlayerDodge>(OnDodge);

    }
}
