using UnityEngine;
using Zenject;

public class npcInteraction : MonoBehaviour
{
    private TextAsset inkAsset;
    private NPC npc;
    [HideInInspector]
    public bool insideInteraction = false;
    public enum typeOfInteraction { Talk, Trade, Quest }
    public typeOfInteraction interactionType;
    private shopManager shopManager;

    [Inject]
    private DialogueService dialogueService;

    void Start()
    {
        npc = GetComponent<NPC>();
        inkAsset = npc.data.dialogue;
        shopManager = FindObjectOfType<shopManager>();
    }
    void OnMouseOver()
    {
        if (!Input.GetKeyDown(KeyCode.I))
            return;
        insideInteraction = true;
        switch (interactionType)
        {
            case typeOfInteraction.Talk:
                dialogueService.Start(inkAsset);
                break;
            case typeOfInteraction.Trade:
                shopManager.ChangeState(this);
                break;
        }
    }
    void OnMouseDown()
    {
        Debug.Log("NPC clicked");
    }
    void Update()
    {
        npc.canMove = !insideInteraction;
    }
}
