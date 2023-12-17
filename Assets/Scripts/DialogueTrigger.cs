using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueList dialogueList;

    public void TriggerDialogue()
    {
        StartCoroutine(GetDialogue());
        StopCoroutine(GetDialogue());
        //dialogueList = FindObjectOfType<JSONReader>().DeserializeJSON(this.gameObject.name);
        //
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogueList);
    }
    
    IEnumerator GetDialogue()
    {
        
        
        dialogueList = FindObjectOfType<JSONReader>().DeserializeJSON(this.gameObject.name);
        Debug.Log("ienum");
        
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueList);
        Debug.LogWarning("dsadasda");
        yield return new WaitForSeconds(0.1f);
    }
}


