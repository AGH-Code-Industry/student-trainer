using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class VisualisingGraph : MonoBehaviour
{
    private MapGraph mapGraph;
    private List<Edge> edges = new List<Edge>();
    public bool visualiseGraph = true;
    void OnDrawGizmos()
    {
        if (!visualiseGraph) return;
        mapGraph = GetComponent<MapGraph>();
        edges = mapGraph.edges;
        foreach (Edge edge in edges)
        {
            if (edge.point1 != null && edge.point2 != null)
            {
                Vector3 startPos = edge.point1.position;
                Vector3 endPos = edge.point2.position;
                float thickness = 10f;
                Handles.DrawBezier(startPos, endPos, startPos, endPos, Color.cyan, null, thickness);
            }
        }
    }
}