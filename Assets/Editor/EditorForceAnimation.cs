using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ForceSceneViewUpdate
{
    static ForceSceneViewUpdate()
    {
        EditorApplication.update += () =>
        {
            if (SceneView.lastActiveSceneView != null)
                SceneView.lastActiveSceneView.Repaint();
        };
    }
}