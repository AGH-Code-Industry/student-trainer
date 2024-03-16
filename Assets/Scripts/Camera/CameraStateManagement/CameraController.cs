using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public GameObject freeCamera;
    public GameObject virtualCamera;
   


    private CameraState currentState;

    private void Start()
    {
        
    freeCamera = GameObject.FindGameObjectWithTag("3rdPersonCamera");
    virtualCamera = GameObject.FindGameObjectWithTag("FreeCamera");
    
    SetState(new FreeCameraState(this));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            if (currentState is FreeCameraState)
            {
                SetState(new VirtualCameraState(this));
            }
            else
            {
                SetState(new FreeCameraState(this));
            }
        }

        currentState.Update();
    }

    public void SetState(CameraState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }
}