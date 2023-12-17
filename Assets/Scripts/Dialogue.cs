using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //[SerializeField]
    public string name;
    
    //[SerializeField]
    public string sentences;
}


[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogues;
}


[System.Serializable]
public class MyTestClass
{
    [SerializeField] 
    public string name = "dziala";
}