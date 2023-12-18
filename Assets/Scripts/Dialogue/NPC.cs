using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    private DialogueTrigger dialogue;

    private void Start() {
        dialogue = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.TriggerDialogue();
        }
    }
}
