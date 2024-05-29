using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class npcMovement : MonoBehaviour
{
    private MapGraph mapGraph;
    public float moveSpeed = 3f;
    public float moveRandomness = 1f;
    private List<Vertex> vertices = new List<Vertex>();
    [HideInInspector]
    public bool isMoving = false;
    private NPC npc;
    void Start()
    {
        npc = GetComponent<NPC>();
        moveSpeed += Random.Range(-moveRandomness, moveRandomness);
        mapGraph = FindObjectOfType<MapGraph>();
        vertices = mapGraph.vertices;
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
            float startX = transform.position.x;
            float startZ = transform.position.z;
            float endX = point.position.x;
            float endZ = point.position.z;
            float timeElapsed = 0;
            float duration = Vector3.Distance(transform.position, point.position) / moveSpeed;
            while (timeElapsed < duration)
            {
                isMoving = npc.canMove;
                if (npc.canMove)
                {
                    float t = timeElapsed / duration;
                    transform.position = new Vector3(
                        Mathf.Lerp(startX, endX, t),
                        0,
                        Mathf.Lerp(startZ, endZ, t) // Movement
                    );
                    timeElapsed += Time.deltaTime;
                }
                Vector3 direction = (point.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                yield return null;
            }

            transform.position = point.position;
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
        Vertex startingVertex = vertices.Find(vertex => vertex.point.position == startingPoint);
        foreach (Vertex vertex in vertices)
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
                Vertex neighbour = vertices.Find(vertex => vertex.point.position == n.position);
                float distance = distances[currentVertex] + Vector3.Distance(currentVertex.point.position, neighbour.point.position);
                if (distance < distances[neighbour])
                {
                    distances[neighbour] = distance;
                    previous[neighbour] = currentVertex;
                }
            }
        }
        Vertex target = vertices.Find(vertex => vertex.point.position == destination);
        if (target != null)
            while (previous.ContainsKey(target))
            {
                path.Add(target.point);
                target = previous[target];
            }
        path.Reverse();
        path.Insert(0, startingVertex.point);
        return path;
    }


    public Vector3 GetClosestPoint(Vector3 position)
    {
        Vector3 closestPoint = Vector3.zero;
        float minDistance = Mathf.Infinity;
        foreach (Vertex vertex in vertices)
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
