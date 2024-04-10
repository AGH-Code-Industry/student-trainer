using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficDirector : MonoBehaviour
{
    public GameObject carPrefab;
    public List<Marker> spawnMarkers;
    public List<Marker> endMarkers;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 7f;

    private int randomSpawnIndex;
    private int randomEndIndex;
   


    private void Start()
    {
        StartCoroutine(waiter());
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

    IEnumerator waiter()
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
        randomSpawnIndex = Random.Range(0, spawnMarkers.Count);
        Marker spawnMarker = spawnMarkers[randomSpawnIndex];
        randomEndIndex = Random.Range(0, endMarkers.Count);
        Marker endMarker = endMarkers[randomEndIndex];

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
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
