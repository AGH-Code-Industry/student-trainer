using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float positionSpeed = 1.0f;
    public float scroll_speed = 1.0f;
    private float scroll;
    void Update()
    {
        
        scroll = Input.GetAxis("Mouse ScrollWheel");
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
            
            // Move only in X and Z directions, keeping Y constant
            transform.Translate(new Vector3(-x * positionSpeed, 0f, -y * positionSpeed), Space.Self);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true; // Make cursor visible again
            Cursor.lockState = CursorLockMode.None; // Free the cursor
        }
    }
}