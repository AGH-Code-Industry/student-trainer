using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanelMenager : MonoBehaviour
{
    public SkillInSlot[] skills_slots;

    public void Start()
    {
        skills_slots = GetComponentsInChildren<SkillInSlot>();
    }

    public void AddSkill(Skill skill)
    {
        for (int i = 0; i < skills_slots.Length; i++)
        {
            if (skills_slots[i].skillInPanel == null)
            {
                skills_slots[i].AddSkillToSlot(skill);
                break;
            }

            if (i == skills_slots.Length - 1)
            {
                Debug.LogError("No free slots");
            }
        }
    }
}
