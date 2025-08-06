using UnityEngine;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour, IInputConsumer
{
    [SerializeField] Animator _animator;

    [SerializeField] Transform _visibleModel;

    Vector3 _currentMovementVector;
    Quaternion _currentModelRotation;

    float _currentX = 0, _currentY = 0;
    [SerializeField] float _interpolationSpeed = 5f;

    private bool interactionPlaying = false;
    [SerializeField] private float interactionAnimDuration = 0.633f;

    [Inject] readonly PlayerService _movement;
    [Inject] readonly EventBus _eventBus;
    [Inject] readonly InputService _inputService;

    void Start()
    {
        //_eventBus.Subscribe<MouseClickUncaught>(OnAttack);
        //_eventBus.Subscribe<PlayerDodge>(OnDodge);
        List<string> wantedActions = new List<string>();
        wantedActions.Add("MouseClick");
        wantedActions.Add("Dodge");
        _inputService.RegisterConsumer(this, wantedActions);
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

    public int priority { get; } = 1;

    public bool ConsumeInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return false;

        if(context.action.name == "MouseClick")
        {
            InputHelper.MouseClickData click = new InputHelper.MouseClickData(context);
            if(click.button == InputHelper.MouseClickData.MouseButton.Left)
            {
                // Don't consume input when only facing the mouse
                FaceMouse();
                return false;
            }
        }
        else if(context.action.name == "Dodge")
        {
            PlayAnimation("Roll");
            return true;
        }

        return false;
    }
    /*
    private void OnDodge(PlayerDodge playerDodge)
    {
        if (playerDodge.ctx.started)
        {
            PlayAnimation("Roll");
        }
    }

    private void OnAttack(MouseClickEvent click)
    {
        bool isClickValid = click.ctx.started && click.button == MouseClickEvent.MouseButton.Left;

        if (isClickValid)
            FaceMouse();
    }
    */
    void FacePosition(Vector3 pos)
    {
        Vector3 direction = (pos - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void FaceMouse()
    {
        var lookVector = _inputService.MouseDownPosition;

        FacePosition(lookVector);
    }

    public void PlayAnimation(string animName)
    {
        _animator.CrossFade(animName, 0.2f);
    }

    public void PlayInteractionAnim(Vector3 sourcePosition)
    {
        if (!interactionPlaying)
        {
            FacePosition(sourcePosition);
            StartCoroutine(InteractionAnim());
        }
    }

    private IEnumerator InteractionAnim()
    {
        interactionPlaying = true;
        _movement.freezer.Freeze("interactionAnim");
        PlayAnimation("Interact");

        yield return new WaitForSeconds(interactionAnimDuration);

        _movement.freezer.Unfreeze("interactionAnim");
        interactionPlaying = false;
    }

    void OnDestroy()
    {
        //_eventBus.Unsubscribe<MouseClickUncaught>(OnAttack);
        //_eventBus.Unsubscribe<PlayerDodge>(OnDodge);
        _inputService.RemoveConsumer(this);
    }
}
