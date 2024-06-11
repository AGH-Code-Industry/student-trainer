using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class VisualisingGraph : MonoBehaviour
{
    private MapGraph mapGraph;
    private List<Edge> edges = new List<Edge>();
    private List<Vertex> vertices = new List<Vertex>();
    public bool visualiseGraph = true;
    public float thickness = 10f;
    public float radius = 5f;
    private RoutineManager routineManager;
    void OnDrawGizmos()
    {
        if (!visualiseGraph) return;
        mapGraph = GetComponent<MapGraph>();
        routineManager = FindObjectOfType<RoutineManager>();
        mapGraph.ConvertEdgesToVertices();
        vertices = mapGraph.vertices;
        edges = mapGraph.edges;

        foreach (Edge edge in edges)
        {
            if (edge.point1 != null && edge.point2 != null)
            {
                Vector3 startPos = edge.point1.position;
                Vector3 endPos = edge.point2.position;
                Handles.DrawBezier(startPos, endPos, startPos, endPos, Color.cyan, null, thickness);
            }
        }
        foreach (Vertex vertex in vertices)
        {
            Handles.DrawSolidDisc(vertex.point.position, Vector3.up, radius);
        }
        Handles.color = Color.red;
        foreach (Task task in routineManager.routines)
        {
            //Handles.DrawSolidDisc(task.destination - new Vector3(0, 2, 0), Vector3.up, radius);
        }
    }
}