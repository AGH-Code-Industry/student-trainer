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

    private void NameChange(Dialogue dialogue)
    {
        foreach (string name in dialogue.name)
        {
            names.Enqueue(name);
        }
    }
    private void TextChange(Dialogue dialogue)
    {
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        
        Debug.Log("Starting convo with " + dialogue.name);
        //nameText.text = dialogue.name;
        names.Clear();
        sentences.Clear();
        NameChange(dialogue);
        TextChange(dialogue);
        
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
