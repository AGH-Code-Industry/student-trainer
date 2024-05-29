using UnityEngine;

public class shopManager : MonoBehaviour
{
    GameObject shopObj;
    public bool isOpen;
    private npcInteraction npcInteraction;
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
        if (isOpen) InputManager.Instance.GetInput().Disable();
        else InputManager.Instance.GetInput().Enable();
    }
}
