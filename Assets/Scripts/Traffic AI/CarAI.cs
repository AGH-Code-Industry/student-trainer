using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarAI : MonoBehaviour
{

    public List<Vector3> path;

    [SerializeField]
    private float arriveDistance = .3f, lastArriveDistance = .5f;
    [SerializeField]
    private float turningAngleOffet = 5;
    [SerializeField]
    private Vector3 currentTargetPosition;
    
    public Marker startMarker, endMarker;

    private int index = 0;
    private bool stop;

    public bool Stop
    {
        get { return stop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    private void Start()
    {
        path = Pathfinder.FindPath(startMarker, endMarker);

        if(path == null || path.Count == 0)
        {
            Stop = true;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if(path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        Vector3 relativePoint = transform.InverseTransformPoint(this.path[index + 1]);
        float angle = Mathf.Atan2(relativePoint.x, relativePoint.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;
    }

    private void Update()
    {
        ChekedIfArrived();
        Drive();
    }

    private void Drive()
    {
        if (Stop == true)
        {
            OnDrive?.Invoke(Vector2.zero);
        }
        else
        {
            Vector3 relativePoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativePoint.x, relativePoint.z) * Mathf.Rad2Deg;
            var rotateCar = 0;
            if (angle > turningAngleOffet)
            {
                rotateCar = 1;
            }
            else if(angle<-turningAngleOffet)
            {
                rotateCar = -1;
            }
            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void ChekedIfArrived()
    {
        if(Stop == false)
        {
            var distanceToCheck = arriveDistance;
            if(index == path.Count - 1)
            {
                distanceToCheck = lastArriveDistance;
            }
            if(Vector3.Distance(currentTargetPosition, transform.position)<distanceToCheck)
            {
                SetNextTargetIndex();
            }

        }
    }

    private void SetNextTargetIndex()
    {
        index++;
        if (index >= path.Count)
        {
            Stop = true;
            Destroy(gameObject);
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }
}
