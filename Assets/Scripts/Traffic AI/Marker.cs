using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Vector3 Position { get => transform.position; }
    public List<Marker> adjacentMarkers;

    public List<Vector3> getAdjacentPositions()
    {
        return new List<Vector3>(adjacentMarkers.Select(x => x.Position).ToList());
    }

    private void OnDrawGizmos()
    {
        if(Selection.activeObject == gameObject)
        {
            if(adjacentMarkers.Count > 0)
            {
                Gizmos.color = Color.red;
                foreach (var item in adjacentMarkers)
                {
                    Gizmos.DrawLine(transform.position, item.Position);
                }
                Gizmos.color = Color.white;
            }
        }
    }

}
