using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FBXImportSettings : AssetPostprocessor
{
    // custom override mapping (e.g. Blender name -> Unity material path)
    static readonly Dictionary<string, string> materialOverrides = new Dictionary<string, string>
    {
        { "Vertex_rough", "Assets/Materials/MatteVtxColor.mat" },
        { "Vertex_Shiny", "Assets/Materials/ShinyVtxColor.mat" },
        { "Vertex_metal", "Assets/Materials/MetalVtxColor.mat" },
        { "Vertex_emmission", "Assets/Materials/EmmissionVtxColor.mat" },
        { "Vertex", "Assets/Materials/VtxColor.mat" }
    };

    void OnPreprocessModel()
    {
        if (assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            var modelImporter = (ModelImporter)assetImporter;

            modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
            modelImporter.materialLocation = ModelImporterMaterialLocation.External;
            modelImporter.materialName = ModelImporterMaterialName.BasedOnTextureName;
            modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
        }
    }

    Material OnAssignMaterialModel(Material defaultMaterial, Renderer renderer)
    {
        string originalName = defaultMaterial.name;

        // First, try to override with custom mapped material
        if (materialOverrides.TryGetValue(originalName, out string overridePath))
        {
            Material customMaterial = AssetDatabase.LoadAssetAtPath<Material>(overridePath);
            if (customMaterial != null)
                return customMaterial;
        }

        // Fallback to Unity's default search
        return null; // Returning null tells Unity to use its standard project-wide search logic
    }
}
