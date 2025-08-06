using UnityEngine;
using Zenject;

public class ChestFiller : MonoBehaviour
{
    [SerializeField] EnemyItemDrop[] items;

    [Inject] readonly InventoryService service;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Container chestContainer = GetComponent<Chest>().container;

        foreach(EnemyItemDrop item in items)
        {
            service.AddItem(item.item, item.amount, chestContainer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
