using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanelMenager : MonoBehaviour
{
    public SkillInSlot[] skillsSlots;

    public void Start()
    {
        skillsSlots = GetComponentsInChildren<SkillInSlot>();
    }

    public void AddSkill(Skill skill)
    {
        for (int i = 0; i < skillsSlots.Length; i++)
        {
            if (skillsSlots[i].skillInPanel == null)
            {
                skillsSlots[i].AddSkillToSlot(skill);
                break;
            }

            if (i == skillsSlots.Length - 1)
            {
                Debug.LogError("No free slots");
            }
        }
    }
}
