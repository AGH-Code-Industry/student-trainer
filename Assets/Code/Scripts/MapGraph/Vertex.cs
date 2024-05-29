using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Vertex
{
    public Transform point;
    public List<Transform> neighbours = new List<Transform>();

    public Vertex(Transform p)
    {
        point = p;
    }
}