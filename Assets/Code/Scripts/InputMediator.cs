using UnityEngine;
using Zenject;

public class InputMediator : MonoBehaviour
{
    Vector3 pos;
    // The height of the plane determines how accurately the player character faces the cursor
    // ... maybe this isn't the besst solution
    readonly Plane groundPlane = new Plane(Vector3.up, -1);

    [Inject] private InputService _service;
    [Inject] private EventBus _eventBus;

    void Start()
    {
        pos = _service.MouseDownPosition;
        _eventBus.Subscribe<MouseClickUncaught>(OnAttack);
    }

    void Update()
    {
        MousePos();
    }

    void MousePos()
    {
        if (Camera.main == null)
            return;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(cameraRay, out float enter))
        {
            pos = cameraRay.GetPoint(enter);
            _service.MouseDownPosition = pos;
        }
    }

    void OnAttack(MouseClickUncaught click)
    {
        bool isClickValid = click.ctx.performed && click.button == MouseClickEvent.MouseButton.Left;
        if (isClickValid)
            _service.GlobalLookTarget = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, 0.5f);
    }

    void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseClickUncaught>(OnAttack);
    }
}