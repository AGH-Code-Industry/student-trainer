using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Quests;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [Inject] readonly QuestService questService;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.nKey.wasPressedThisFrame)
        {
            questService.ActivateQuest("demo_quest");
        }
    }
}
