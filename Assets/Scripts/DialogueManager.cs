using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    
    private Queue<string> names;
    private Queue<string> sentences;

    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();
    }

    private void NameChange(DialogueList dialogueList) //Dialogue dialogue
    {
        for (int i = 0; i < dialogueList.dialogues.Length; i++)
        {
            names.Enqueue(dialogueList.dialogues[i].name);
        }
        /*foreach (string name in dialogueList.dialogues[].name) //string name in dialogue.name
        {
            names.Enqueue(name);
        }*/
    }
    private void TextChange(DialogueList dialogueList)
    {
        for (int i = 0; i < dialogueList.dialogues.Length; i++)
        {
            names.Enqueue(dialogueList.dialogues[i].sentences);
        }
        /*foreach (string sentence in dialogue.sentences) //string sentence in dialogue.sentences
        {
            sentences.Enqueue(sentence);
        }*/
    }
    public void StartDialogue(DialogueList dialogueList) //Dialogue dialogue
    {
        animator.SetBool("IsOpen", true);
        
        Debug.Log("Starting convo with " + dialogueList.dialogues[0].name);
        //nameText.text = dialogue.name;
        names.Clear();
        sentences.Clear();
        NameChange(dialogueList);
        TextChange(dialogueList);
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeName(name));
        StartCoroutine(TypeSentence(sentence));
        //Debug.Log(sentence);

        //dialogueText.text = sentence;
    }

    IEnumerator TypeName(string name)
    {
        nameText.text = "";
        foreach (char letter in name.ToCharArray())
        {
            nameText.text += letter;
            yield return null;
        }
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("End of convo");
    }
}
