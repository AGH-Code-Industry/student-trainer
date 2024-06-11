using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class TrafficDirector : MonoBehaviour
{
    public GameObject carPrefab;
    public List<Marker> spawnMarkers;
    public List<Marker> endMarkers;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 7f;

    private int randomSpawnIndex;
    private int randomEndIndex;

    private Scene carScene;

    private void Start()
    {
        StartCoroutine(spawnCounter());
    }

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > period)
    //    {
    //        TrySpawnCar();
    //        timer = 0;
    //    }
    //}

    IEnumerator spawnCounter()
    {
        while (true)
        {
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
            TrySpawnCar();
        }

    }

    private void TrySpawnCar()
    {
        for (int i = 0; i < spawnMarkers.Count; i++)
        {
            Marker spawnMarker = spawnMarkers[i];
            Marker endMarker = endMarkers[i];

            Vector3 facingDirection = spawnMarker.adjacentMarkers[0].Position - spawnMarker.Position;
            facingDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(facingDirection);


            GameObject car = Instantiate(carPrefab, spawnMarker.Position, rotation);
            CarAI carAI = car.GetComponent<CarAI>();
            if (carAI != null)
            {
                carAI.startMarker = spawnMarker;
                carAI.endMarker = endMarker;
            }
            MoveToCarScene(car);
        }

    }

    private void MoveToCarScene(GameObject o)
    {
        if (!carScene.isLoaded)
        {
            if (Application.isEditor)
            {
                carScene = SceneManager.GetSceneByName(name);
                if (!carScene.isLoaded)
                {
                    carScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                carScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, carScene);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
