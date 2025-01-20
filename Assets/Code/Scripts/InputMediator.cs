using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputMediator : MonoBehaviour
{
    PlayerInput _playerInput;

    Vector3 pos;

    // The height of the plane determines how accurately the player character faces the cursor
    // ... maybe this isn't the besst solution
    readonly Plane groundPlane = new Plane(Vector3.up, -1);

    [Inject] readonly InputService _service;
    [Inject] readonly PlayerCombatService _combat;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        MousePos();
    }

    void MousePos()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;

        if(groundPlane.Raycast(cameraRay, out enter))
        {
            pos = cameraRay.GetPoint(enter);
            _service.SetLookTarget(pos);
        }
    }

    void OnMove(InputValue val)
    {
        Vector2 input = val.Get<Vector2>();
        _service.SetMovementVector(input);
    }

    void OnAttack()
    {
        _combat.Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, 0.5f);
    }
}