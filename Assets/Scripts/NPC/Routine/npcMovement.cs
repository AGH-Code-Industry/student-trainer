using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class npcMovement : MonoBehaviour
{
    private MapGraph mapGraph;
    private float moveSpeed = 2f;
    private float stoppingDistance = 0.1f;
    private List<Vertex> vertices = new List<Vertex>();
    [HideInInspector]
    public bool isMoving = false;
    void Start()
    {
        mapGraph = FindObjectOfType<MapGraph>();
    }

    public void Move(Task task)
    {
        StartCoroutine(MoveToDestination(task));
    }

    public IEnumerator MoveToDestination(Task task)
    {
        Vector3 destination = task.destination;
        List<Transform> path = FindPath(destination);
        foreach (Transform point in path)
        {
            while (Vector3.Distance(transform.position, point.position) > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, moveSpeed * Time.deltaTime);
                yield return 0.1;
            }
        }
        isMoving = false;
    }
    public List<Transform> FindPath(Vector3 destination)
    {
        Vector3 startingPoint = GetClosestPoint(transform.position);
        return Dijkstra(startingPoint, destination);
    }

    public List<Transform> Dijkstra(Vector3 startingPoint, Vector3 destination)
    {
        List<Transform> path = new List<Transform>();
        Dictionary<Vertex, float> distances = new Dictionary<Vertex, float>();
        Dictionary<Vertex, Vertex> previous = new Dictionary<Vertex, Vertex>();
        List<Vertex> unvisited = new List<Vertex>();
        foreach (Vertex vertex in mapGraph.vertices)
        {
            if (vertex.point.position == startingPoint)
            {
                distances[vertex] = 0;
            }
            else
            {
                distances[vertex] = Mathf.Infinity;
            }
            unvisited.Add(vertex);
        }
        while (unvisited.Count > 0)
        {
            Vertex currentVertex = unvisited.OrderBy(vertex => distances[vertex]).First();
            unvisited.Remove(currentVertex);
            foreach (Transform n in currentVertex.neighbours)
            {
                Vertex neighbour = mapGraph.vertices.Find(vertex => vertex.point.position == n.position);
                float distance = distances[currentVertex] + Vector3.Distance(currentVertex.point.position, neighbour.point.position);
                if (distance < distances[neighbour])
                {
                    distances[neighbour] = distance;
                    previous[neighbour] = currentVertex;
                }
            }
        }
        Vertex target = mapGraph.vertices.Find(vertex => vertex.point.position == destination);
        while (previous.ContainsKey(target))
        {
            path.Add(target.point);
            target = previous[target];
        }
        path.Reverse();
        return path;
    }


    public Vector3 GetClosestPoint(Vector3 position)
    {
        Vector3 closestPoint = Vector3.zero;
        float minDistance = Mathf.Infinity;
        foreach (Vertex vertex in mapGraph.vertices)
        {
            float distance = Vector3.Distance(position, vertex.point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = vertex.point.position;
            }
        }
        return closestPoint;
    }
}
