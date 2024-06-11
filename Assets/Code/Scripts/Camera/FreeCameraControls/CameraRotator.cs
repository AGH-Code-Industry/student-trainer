using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // Adjust the speed of rotationpublic float rotationSpeed = 100.0f;
    

    private float verticalRotation = 0.0f; // Track vertical rotation separately


  

    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Cursor.visible = false; // Make cursor invisible
            Cursor.lockState = CursorLockMode.Locked; // Optionally lock the cursor to the center of the screen
            //rotation

            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Apply horizontal rotation around the Y axis
            transform.Rotate(Vector3.up, mouseX, Space.World);

            // Adjust and clamp the vertical rotation to prevent flipping
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            // Apply the vertical rotation around the X axis independently
            transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0);

        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true; // Make cursor visible
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        }
       
    }

   
}