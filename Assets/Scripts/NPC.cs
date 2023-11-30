using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger!");
        if (other.CompareTag("Player"))
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
