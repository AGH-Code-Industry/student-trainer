using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mouse : MonoBehaviour
{
    private Vector3 offset = new Vector3(-8, -0.2f, 0);
    public Camera cam;
    public Vector3 position;
    void Update()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        position = new Vector3(pos.x, pos.y, 0) + offset;
    }
    public bool IsOver(Transform child, Vector3 mousePosition, Vector3 rect)
    {
        return child.position.x + rect.x / 2 > mousePosition.x &&
               child.position.x - rect.x / 2 < mousePosition.x &&
               child.position.y + rect.y / 2 > mousePosition.y &&
               child.position.y - rect.y / 2 < mousePosition.y;
    }
}