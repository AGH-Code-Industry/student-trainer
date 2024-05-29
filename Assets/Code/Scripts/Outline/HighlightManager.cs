using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightManager : MonoBehaviour
{
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask selectableLayer;
    
    private Outline highlightOutline;
    private RaycastHit hit;
    
    public float detectionRange = 3.0f;  // Ustaw zasięg wykrywania
    private List<Transform> enemies;     // Lista do przechowywania transformacji wrogów

    void Start()
    {
        // Wyszukaj wszystkie obiekty z tagiem "Enemy" i dodaj je do listy
        enemies = new List<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy.transform);
        }
    }
    
    
    

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Transform enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
            if (distanceToEnemy <= detectionRange)
            {
                HoverHighlight();
            }
        }
    }

    private void HoverHighlight()
    {
        if (highlightedObj != null)
        {
            highlightOutline.enabled = false;
            highlightedObj = null;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform;
            
            if(highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();
                highlightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
        
        
    }

    public void SelectedHighlight()
    {
        if(highlightedObj != null)
        {
            if (highlightedObj.CompareTag("Enemy"))
            {
                if (selectedObj != null)
                {
                    selectedObj.GetComponent<Outline>().enabled = false;
                }

                selectedObj = hit.transform;
                selectedObj.GetComponent<Outline>().enabled = true;

                highlightOutline.enabled = true;
                highlightedObj = null;
            }
        }
    }
    
    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectedObj = null;
        }
    }
}
