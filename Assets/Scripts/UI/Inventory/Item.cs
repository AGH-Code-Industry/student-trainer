using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDescription;
    private Mouse mouse;
    public Transform toRotate;
    public Vector3 rect;
    public Vector3 axisSpeed;
    void Start()
    {
        mouse = FindObjectOfType<Mouse>();
    }
    void FixedUpdate()
    {
        if (mouse.IsOver(transform, mouse.position, rect))
        {
            toRotate.Rotate(axisSpeed.x, axisSpeed.y, axisSpeed.z);
        }
    }
}
