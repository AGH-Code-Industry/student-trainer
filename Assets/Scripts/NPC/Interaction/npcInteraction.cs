using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcInteraction : MonoBehaviour
{
    private Color defaultColor;
    private List<SkinnedMeshRenderer> childRenderers;
    private Color highlightColor = Color.yellow;
    private TextAsset inkAsset;
    private NPC npc;
    private bool highlighted = false;
    [HideInInspector]
    public bool insideInteraction = false;
    public enum typeOfInteraction { Talk, Trade, Quest }
    public typeOfInteraction interactionType;
    private shopManager shopManager;
    void Start()
    {
        childRenderers = new List<SkinnedMeshRenderer>();
        GetComponentsInChildren<SkinnedMeshRenderer>(true, childRenderers);
        defaultColor = childRenderers[0].material.color;
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
                DialogueManager.Instance.StartDialogue(inkAsset);
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
        if (highlighted && !insideInteraction)
            Highlight();
        else
            Unhighlight();

    }
    void OnMouseEnter()
    {
        highlighted = true;
    }
    void OnMouseExit()
    {
        highlighted = false;
    }
    void Highlight()
    {
        foreach (SkinnedMeshRenderer renderer in childRenderers)
            renderer.material.color = highlightColor;
    }
    void Unhighlight()
    {
        foreach (SkinnedMeshRenderer renderer in childRenderers)
            renderer.material.color = defaultColor;
    }
}
