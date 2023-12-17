using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.Json;

public class JSONReader : MonoBehaviour
{
    private const string filePath = "Assets/Data/JSON/";
    public TextAsset textJSON;
    public DialogueList myDialogue = new DialogueList();
    public MyTestClass myClass = new MyTestClass();
    private void Start()
    {
        Debug.Log("start");
        Debug.Log("wr "+ JsonUtility.FromJson<Dialogue>(textJSON.text).sentences);
        myDialogue = JsonUtility.FromJson<DialogueList>(textJSON.text);
        myClass = JsonUtility.FromJson<MyTestClass>(textJSON.text);
    }

    void DeserializeJSON(string name)
    {
            
        
        
            /*string fileName = name + ".json";
            TextAsset textJSON = UnityEngine.Windows.File(fileName);
            using FileStream openStream = File.OpenRead(fileName);
            Dialogue? dialogue = 
                await JsonSerializer.DeserializeAsync<Dialogue>(openStream);

            Debug.Log($"Date: {dialogue?.name[0]}");
            Debug.Log($"TemperatureCelsius: {dialogue?.sentences[0]}");*/
            //Debug.Log($"Summary: {dialogue?.name}");
        
    }
}
