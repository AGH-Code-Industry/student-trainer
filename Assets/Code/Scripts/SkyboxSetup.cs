using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSetup : MonoBehaviour
{
    [SerializeField] Material skyboxMaterial;
    [SerializeField] Color ambientLightColor;

    void Start()
    {
        RenderSettings.skybox = skyboxMaterial;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientSkyColor = ambientLightColor;
    }

    void Update()
    {
        
    }
}