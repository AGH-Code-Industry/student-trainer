using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    
    public float positionspeed = 1.0f;
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false; // Make cursor invisible
            Cursor.lockState = CursorLockMode.Locked; // Optionally lock the cursor to the center of the screen
        }
        
        if (Input.GetMouseButton(0))
        {
           
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            
            transform.Translate(new Vector3(-x * positionspeed, 0f, -y * positionspeed), Space.Self);


        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true; // Make cursor visible again
            Cursor.lockState = CursorLockMode.None; // Free the cursor
        }
    }
}
