using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string followObjectWithTag = "Player";
    public float smoothSpeed = 0.125f;
	public Vector3 rotationOffset;
	public Vector3 positionOffset;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag(followObjectWithTag).FirstOrDefault()?.transform ?? null;
    }


	void FixedUpdate ()
	{
        if(target == null) return;

		Vector3 desiredPosition = target.position +
        ((target.forward * rotationOffset.z) + (target.up * rotationOffset.y) + (target.right * rotationOffset.x));
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

		transform.LookAt(target.position + positionOffset);
	}
}
