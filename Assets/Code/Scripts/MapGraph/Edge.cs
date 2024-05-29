using UnityEngine;
[System.Serializable]
public class Edge
{
    public Transform point1;
    public Transform point2;

    public Edge(Transform p1, Transform p2)
    {
        point1 = p1;
        point2 = p2;
    }
}