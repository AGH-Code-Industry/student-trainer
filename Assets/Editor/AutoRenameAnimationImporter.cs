using UnityEngine;
using UnityEditor;
using System.Linq;

public class AutoRenameAnimationImporter : AssetPostprocessor
{
    void OnPostprocessModel(GameObject g)
    {
        ModelImporter importer = assetImporter as ModelImporter;

        // Get the file name without extension
        string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

        // Get all clip animations imported from the model
        ModelImporterClipAnimation[] clips = importer.clipAnimations;

        // If clipAnimations is empty, Unity might be using defaultClipAnimations
        if (clips == null || clips.Length == 0)
            clips = importer.defaultClipAnimations;

        bool modified = false;

        foreach (var clip in clips)
        {
            if (clip.name == "Scene")
            {
                clip.name = fileName;
                modified = true;
            }
        }

        if (modified)
        {
            importer.clipAnimations = clips;
            Debug.Log($"[{fileName}] Renamed animation clip(s) to '{fileName}'.");
        }
    }
}
