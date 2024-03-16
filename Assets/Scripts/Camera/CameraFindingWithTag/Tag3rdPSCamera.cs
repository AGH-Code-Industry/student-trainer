using UnityEngine;
using Cinemachine;

public class Tag3rdPSCamera : MonoBehaviour
{
    public string PlayerTag = "Player"; // The tag of your target object
    
    
    
    private CinemachineFreeLook _3PersonCamera;
  

    void Start()
    {
        // Find the target by tag
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
     
        
        if (player != null)
        {
            // Get the Cinemachine Virtual Camera component
            _3PersonCamera = GetComponent<CinemachineFreeLook>();
            
            // Set the target object as the Follow target
            _3PersonCamera.Follow = player.transform;
           
            // Set the target object as the Look At target
            _3PersonCamera.LookAt = player.transform;
            
        }
        else
        {
            Debug.LogError("Target not found. Make sure your target is tagged correctly.");
        }
    }
}