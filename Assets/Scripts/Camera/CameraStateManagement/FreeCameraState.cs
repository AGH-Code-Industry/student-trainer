using UnityEngine;
public class FreeCameraState : CameraState
{
    public FreeCameraState(CameraController controller) : base(controller) {}
    
    

    public override void Enter()
    {
        controller.freeCamera.SetActive(true);
        controller.virtualCamera.gameObject.SetActive(false);
        // Initialize anything specific to the Free Camera state
        

    }

    public override void Exit()
    {
        // Clean up the state as needed
    }
    
    public override void Update()
    {
        // Handle update logic for Free Camera
    }
}