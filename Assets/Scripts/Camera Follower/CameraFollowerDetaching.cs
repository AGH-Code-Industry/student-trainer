using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook camera1;
    public CinemachineVirtualCamera camera2;
    
    public GameObject Casual_2;
    public GameObject FreeCameraCin;
   

    private bool inheriting_transform = false;

    private void Start()
    {
        transform.parent = null;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(2)) // scroll
        {
            inheriting_transform = !inheriting_transform;

            if (inheriting_transform)
            {
                transform.position = Casual_2.transform.position;
                transform.position = new Vector3(transform.position.x, 20, transform.position.z);
                Vector3 rotacja = new Vector3(45, transform.rotation.y, transform.rotation.z);
                GetComponent<CameraFollowerController>().enabled = true;
                
                
            }
            else
            {
                GetComponent<CameraFollowerController>().enabled = false;
            }

            // Check which camera is currently active
            if (camera1.Priority > camera2.Priority)
            {
                // If camera1 is active, switch to camera2
                camera1.Priority = 0;
                camera2.Priority = 1;
            }
            else
            {
                // If camera2 is active or none, switch to camera1
                camera1.Priority = 1;
                camera2.Priority = 0;
            }
            

           
        }

        
        
       

            
        
    }
    
 
}
