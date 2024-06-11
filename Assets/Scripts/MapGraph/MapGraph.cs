using System.Collections.Generic;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
  [SerializeField] public List<Vertex> vertices = new List<Vertex>();
  [SerializeField] public List<Edge> edges = new List<Edge>();
  void Start()
  {
    ConvertEdgesToVertices();
  }
  public void ConvertEdgesToVertices()
  {
    vertices.Clear();
    foreach (Edge edge in edges)
    {
      if (vertices.Find(v => v.point == edge.point1) == null)
      {
        vertices.Add(new Vertex(edge.point1));
      }
      if (vertices.Find(v => v.point == edge.point2) == null)
      {
        vertices.Add(new Vertex(edge.point2));
      }
    }
    foreach (Edge edge in edges)
    {
      Vertex v1 = vertices.Find(v => v.point == edge.point1);
      Vertex v2 = vertices.Find(v => v.point == edge.point2);
      v1.neighbours.Add(edge.point2);
      v2.neighbours.Add(edge.point1);
    }
  }
}