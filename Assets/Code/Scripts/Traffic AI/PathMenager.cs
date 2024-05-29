using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public static List<Vector3> FindPath(Marker startMarker, Marker targetMarker)
    {
        List<Vector3> path = new List<Vector3>();
        HashSet<Marker> visited = new HashSet<Marker>();

        // Perform DFS traversal
        DFS(startMarker, targetMarker, visited, path);

        return path;
    }

    private static bool DFS(Marker currentMarker, Marker targetMarker, HashSet<Marker> visited, List<Vector3> path)
    {
        if (currentMarker == targetMarker)
        {
            path.Insert(0, currentMarker.Position);
            return true;
        }

        visited.Add(currentMarker);

        foreach (Marker neighborMarker in currentMarker.adjacentMarkers)
        {
            if (!visited.Contains(neighborMarker))
            {
                if (DFS(neighborMarker, targetMarker, visited, path))
                {
                    path.Insert(0, currentMarker.Position);
                    return true;
                }
            }
        }

        return false;
    }
}