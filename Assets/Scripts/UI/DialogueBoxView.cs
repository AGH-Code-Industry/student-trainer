using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxView : MonoBehaviour 
{
    public static readonly int HEIGHT = 500;
    public static readonly int FIRST_ELEMENT_OFFSET = -250;

    [Header("Target")]
    public TextMeshProUGUI targetName;
    public TextMeshProUGUI targetDialogue;
    public Image targetImage;

    [Header("You")]
    public Image yourImage;
    public GameObject[] yourChoices;
    private TextMeshProUGUI[] yourChoicesText;

    public void SetDialogue(DialogueBoxData data)
    {
        yourChoicesText = yourChoices.Select(c => c.GetComponentInChildren<TextMeshProUGUI>()).ToArray();

        targetName.text = data.targetName;
        targetDialogue.text = data.targetDialogue;

        // targetImage = Resources.Load
        // yourImage = Resources.Load

        if(data.yourChoices.Length == 0)
            data.yourChoices = new string[]{"( Kontynuuj )"};

        yourChoices.Select(c => {c.SetActive(false); return c;}).ToArray();
        for(int i = 0; i < math.clamp(data.yourChoices.Length, 0, 4); i++)
        {
            yourChoices[i].SetActive(true);
            yourChoicesText[i].text = data.yourChoices[i];
        }
    }

    public void OnSelectDialogueChoice(int choiceIndex) 
    {
        yourChoices.Select(c => c.GetComponent<Button>().enabled = false);
        yourChoices[choiceIndex].GetComponent<Button>().Select();

        UIEvents.SelectedDialogueChoice.Invoke(choiceIndex);
    }
    
}
