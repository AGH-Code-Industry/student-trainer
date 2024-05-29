using Cinemachine;
using UnityEngine;
public class Person3Camera : CameraState
{
    public Person3Camera(CameraController controller) : base(controller) {}

    public GameObject _3rdPersonCamera;
    

    public override void Enter()
    {
        _3rdPersonCamera = GameObject.FindGameObjectWithTag("3rdPersonCamera");
        controller.freelookCamera.SetActive(true);
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
        if(_3rdPersonCamera == null)
        {
            _3rdPersonCamera = GameObject.FindGameObjectWithTag("3rdPersonCamera");
        }
        else
        {

            if (Input.GetMouseButtonDown(1))
            {
                _3rdPersonCamera.GetComponent<CinemachineInputProvider>().enabled = true;
            }


            if (Input.GetMouseButtonUp(1))
            {
                _3rdPersonCamera.GetComponent<CinemachineInputProvider>().enabled = false;
            }

        }
    }
}