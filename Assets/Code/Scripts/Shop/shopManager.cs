using UnityEngine;
using Zenject;

public class shopManager : MonoBehaviour
{
    GameObject shopObj;
    public bool isOpen;
    private npcInteraction npcInteraction;

    [Inject]
    private InputService input;

    public void Start()
    {
        shopObj = GameObject.Find("Shop");
        isOpen = false;
        shopObj.SetActive(false);
    }
    public void ChangeState(npcInteraction tempInteraction)
    {
        npcInteraction = tempInteraction;
        shopObj.SetActive(!shopObj.activeInHierarchy);
        isOpen = !isOpen;
        npcInteraction.insideInteraction = isOpen;
        input.SetActive(!isOpen);
    }
}
