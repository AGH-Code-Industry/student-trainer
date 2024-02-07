using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxView : MonoBehaviour 
{
    public static readonly int HEIGHT = 250;

    [Header("Npc")]
    public GameObject npc;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public Image npcImage;

    [Header("Hero")]
    public GameObject hero;
    public TextMeshProUGUI heroName;
    public TextMeshProUGUI heroDialogue;
    public Image heroImage;
    public GameObject[] heroChoices;
    public GameObject end;

    private TextMeshProUGUI[] heroChoicesText;

    public void SetDialogue(DialogueBoxData data)
    {
        npc.SetActive(false);
        hero.SetActive(false);

        if(data.type == DialogueType.Npc)
        {
            npc.SetActive(true);
            npcName.text = data.name;
            npcDialogue.text = data.dialogue;
            StartCoroutine("WaitAndPrint");
        }
        else if(data.type == DialogueType.Hero_Dialogue || data.type == DialogueType.Hero_Choices)
        {
            hero.SetActive(true);

            heroName.text = data.name;

            if (data.type == DialogueType.Hero_Dialogue)
            {
                heroDialogue.enabled = true;
                heroDialogue.text = data.dialogue;
                StartCoroutine("WaitAndPrint");
            }
            else if(data.type == DialogueType.Hero_Choices)
            {
                heroChoicesText = heroChoices.Select(c => c.GetComponentInChildren<TextMeshProUGUI>()).ToArray();
                for(int i = 0; i < math.clamp(data.choices.Length, 0, 4); i++)
                {
                    heroChoices[i].SetActive(true);
                    heroChoicesText[i].text = data.choices[i];
                }
            }
        }
        else if(data.type == DialogueType.End)
        {
            hero.SetActive(true);
            end.SetActive(true);
        }
    }

    private IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(1);
        print("Coroutine ended: " + Time.time + " seconds");
        UIEvents.NextDialogue.Invoke();
    }

    public void OnSelectDialogueChoice(int choiceIndex) 
    {   
        foreach(var choice in heroChoices)
        {
            choice.GetComponent<Button>().enabled = false;
            choice.GetComponent<Image>().enabled = false;
        }

        heroChoices[choiceIndex].GetComponent<Button>().interactable = false;
        heroChoices[choiceIndex].GetComponent<Button>().enabled = true;
        heroChoices[choiceIndex].GetComponent<Image>().enabled = true;

        UIEvents.SelectedDialogueChoice.Invoke(choiceIndex);
    }

    public void OnSelectEnd()
    {
        UIEvents.CloseDialogue.Invoke();
    }
    
}
