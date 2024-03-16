using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public GameObject freelookCamera;
    public GameObject virtualCamera;
   


    private CameraState currentState;

    private void Start()
    {
    
            
    freelookCamera = GameObject.FindGameObjectWithTag("3rdPersonCamera");
    virtualCamera = GameObject.FindGameObjectWithTag("FreeCamera");
    
    SetState(new Person3Camera(this));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            if (currentState is Person3Camera)
            {
                SetState(new VirtualCameraState(this));
            }
            else
            {
                SetState(new Person3Camera(this));
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