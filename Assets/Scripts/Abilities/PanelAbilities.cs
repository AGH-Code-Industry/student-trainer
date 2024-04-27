using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Abilities;

public class panel_abilities : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabActive;
    public Sprite tabIdle;
    public Sprite tabHover;
    public TabButton selectedTab;
    public List<GameObject> ObjectsToChoose;

    public static event Action<SkillsEnum> Skillsy;

    public static event Action<string> OnSkillClicked;

    public void OnSkillButtonClicked(string skillName)
    {
        OnSkillClicked?.Invoke(skillName);
        Debug.Log(skillName);
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
        button.background.sprite = tabIdle;
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
        /*int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < ObjectsToChoose.Count; i++)
        {
            if (i == index)
            {
                ObjectsToChoose[i].SetActive(true);
            }
            else
            {
                ObjectsToChoose[i].SetActive(false);
            }
        }*/
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
