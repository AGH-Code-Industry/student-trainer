using UnityEngine;
using System.Collections.Generic;

public class EntityHurtboxes : MonoBehaviour
{
    [SerializeField] private List<GameObjectEntry> hurboxEntries;
    [SerializeField] private List<TransformEntry> anchorEntries;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Write a property drawer for dictionaries

    [System.Serializable]
    private struct GameObjectEntry
    {
        public string id;
        public GameObject value;
    }

    [System.Serializable]
    private struct TransformEntry
    {
        public string id;
        public Transform value;
    }
}
