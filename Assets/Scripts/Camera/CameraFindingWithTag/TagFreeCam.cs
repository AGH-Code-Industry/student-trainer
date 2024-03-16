using UnityEngine;
using Cinemachine;

public class TagFreeCam : MonoBehaviour
{
    
    public string CameraFollowerTag = "CameraFollower"; // The tag of your target object
    
    

    private CinemachineVirtualCamera FreeCamera;

    void Start()
    {
        // Find the target by tag
        
        GameObject camera_follower = GameObject.FindGameObjectWithTag(CameraFollowerTag);
        
        if (camera_follower != null)
        {
            // Get the Cinemachine Virtual Camera component
            
            FreeCamera = GetComponent<CinemachineVirtualCamera>();
            // Set the target object as the Follow target
           
            FreeCamera.Follow = camera_follower.transform;
            // Set the target object as the Look At target
          
            FreeCamera.LookAt = camera_follower.transform;
        }
        else
        {
            Debug.LogError("Target not found. Make sure your target is tagged correctly.");
        }
    }
}