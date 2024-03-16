
public class VirtualCameraState : CameraState
{
    public VirtualCameraState(CameraController controller) : base(controller) {}

    public override void Enter()
    {
        controller.virtualCamera.gameObject.SetActive(true);
        controller.freeCamera.SetActive(false);
        // Initialize anything specific to the Virtual Camera state
    }

    public override void Exit()
    {
        // Clean up the state as needed
    }

    public override void Update()
    {
        // Handle update logic for Virtual Camera
    }
}