using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Quests;
using TMPro;
using UnityEngine.UI;

// Right now it can only track a single quest! Will need to be rewritten

public class QuestListUI : MonoBehaviour
{
    [SerializeField] GameObject questStepPrefab, separator, stepsList;
    [SerializeField] TextMeshProUGUI waitingText, activeText, popupText;

    [SerializeField] Color activeColor, inactiveColor, inProgressColor, completedColor, failedColor, popupColor;

    [Inject] readonly EventBus eventBus;
    [Inject] readonly QuestService questService;

    string trackedQuestID = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideAll();
        popupText.gameObject.SetActive(false);

        eventBus.Subscribe<QuestStatusChanged>(OnQuestChanged);
        eventBus.Subscribe<StepStatusChanged>(OnStepChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HideAll()
    {
        waitingText.gameObject.SetActive(false);
        separator.SetActive(false);
        activeText.gameObject.SetActive(false);
        stepsList.SetActive(false);
    }

    void UpdateUI()
    {
        if(string.IsNullOrEmpty(trackedQuestID))
        {
            HideAll();
            return;
        }

        Quest trackedQuest = questService.GetQuestByID(trackedQuestID);

        activeText.gameObject.SetActive(true);
        activeText.text = trackedQuest.name;

        stepsList.SetActive(true);

        foreach(Transform child in stepsList.transform)
        {
            Destroy(child.gameObject);
        }

        QuestStepBase[] steps = trackedQuest.steps;
        int i = 0;
        foreach(QuestStepBase step in steps)
        {
            i++;
            if (step.status == QuestStepStatus.Waiting)
            {
                print($"Step {i} is waiting.");
                continue;
            }

            GameObject spawned = Instantiate(questStepPrefab, stepsList.transform);

            Image bg = spawned.GetComponentInChildren<Image>();
            TextMeshProUGUI text = spawned.GetComponentInChildren<TextMeshProUGUI>();

            string description = step.GetStepText();

            print($"Printing step {i}");

            if(step.status == QuestStepStatus.Ongoing)
            {
                text.text = description;
                text.color = activeColor;
                bg.color = inProgressColor;
            }
            else if(step.status == QuestStepStatus.Completed)
            {
                text.text = $"<s>{description}</s>";
                text.color = inactiveColor;
                bg.color = completedColor;
            }
            else if(step.status == QuestStepStatus.Failed)
            {
                text.text = $"<s>{description}</s>";
                text.color = inactiveColor;
                bg.color = failedColor;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(stepsList.GetComponent<RectTransform>());
    }

    void OrderHierarhy()
    {

    }

    IEnumerator ShowPopup(string message, string questID)
    {
        popupText.gameObject.SetActive(true);

        TextMeshProUGUI textComponent = popupText.GetComponent<TextMeshProUGUI>();

        string questName = questService.GetQuestByID(questID).name;
        string toDisplay = message + "\n<color=#" + ColorUtility.ToHtmlStringRGB(popupColor) + ">" + questName + "</color>";

        textComponent.text = toDisplay;

        yield return new WaitForSecondsRealtime(4f);

        popupText.gameObject.SetActive(false);
    }

    void OnQuestChanged(QuestStatusChanged ev)
    {
        if(ev.questID != trackedQuestID)
        {
            if (ev.newStatus == QuestStatus.Ongoing)
            {
                trackedQuestID = ev.questID;
                StartCoroutine(ShowPopup("Nowe zadanie rozpoczęte", trackedQuestID));
            }
        }
        else
        {
            if(ev.newStatus == QuestStatus.Completed)
            {
                StartCoroutine(ShowPopup("Zadanie ukończone", trackedQuestID));
                trackedQuestID = "";
            }
            else if(ev.newStatus == QuestStatus.Failed)
            {
                // Nie wiem jak napisać failed po polsku xd
                StartCoroutine(ShowPopup("Zadanie nie powiodło się", trackedQuestID));
                trackedQuestID = "";
            }
        }

        UpdateUI();
    }

    void OnStepChanged(StepStatusChanged ev)
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<QuestStatusChanged>(OnQuestChanged);
        eventBus.Unsubscribe<StepStatusChanged>(OnStepChanged);
    }
}
