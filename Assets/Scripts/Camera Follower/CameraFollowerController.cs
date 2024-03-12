using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraFollowerController : MonoBehaviour
{
    

    public float scroll_speed = 1.0f;

    public GameObject camera_rotator; 
    
    public float positionspeed = 1.0f;
    

    void Update()
    {
        //tranformation


//scroll wheel hight 
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += new Vector3(0, scroll * scroll_speed, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false; // Make cursor invisible
            Cursor.lockState = CursorLockMode.Locked; // Optionally lock the cursor to the center of the screen
        }
        
        if (Input.GetMouseButton(0))
        {
           
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            
            transform.Translate(new Vector3(-x * positionspeed, 0, -y * positionspeed), Space.Self);


        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true; // Make cursor visible again
            Cursor.lockState = CursorLockMode.None; // Free the cursor
        }
       
            


        if (Input.GetMouseButtonDown(1))
        {
            camera_rotator.transform.SetParent(null);
            camera_rotator.GetComponent<CameraRotator>().enabled = true;
            
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            camera_rotator.GetComponent<CameraRotator>().enabled = false;
            
           transform.localEulerAngles = new Vector3(0, camera_rotator.transform.localEulerAngles.y, 0);
           
            
            Cursor.visible = true; // Make cursor visible again
            Cursor.lockState = CursorLockMode.None; // Free the cursor
            
            camera_rotator.transform.SetParent(transform);
       
        }
    }
}