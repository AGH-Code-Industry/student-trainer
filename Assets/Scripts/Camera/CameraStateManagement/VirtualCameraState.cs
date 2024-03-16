
using Cinemachine;
using UnityEditor.SceneManagement;
using UnityEditor.TextCore.Text;
using UnityEngine;
using Component = System.ComponentModel.Component;

public class VirtualCameraState : CameraState
{
    public VirtualCameraState(CameraController controller) : base(controller) {}
       
    public GameObject camera_follower= GameObject.FindGameObjectWithTag("CameraFollower");
    
    public GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    private Vector3 startingPosition;
    private Quaternion rotation_eangle;
    
    public override void Enter()
    {
        // Set the position to player position
     
        startingPosition = player.transform.position;
        camera_follower.transform.position = new Vector3(startingPosition.x, startingPosition.y + 10f, startingPosition.z - 3.5f);
        
        
        controller.virtualCamera.gameObject.SetActive(true);
        controller.freeCamera.SetActive(false);
        // Initialize anything specific to the Virtual Camera state
      
       camera_follower.GetComponent<CameraRotator>().enabled = true;
    }

    public override void Exit()
    {
        // Clean up the state as needed
        camera_follower.GetComponent<CameraRotator>().enabled = false;
    }

    
    public override void Update()
    {
        // Handle update logic for Virtual Camera
    }
}