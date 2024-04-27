using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills_panel_Menager : MonoBehaviour
{
    public Slot[] skills_slots;

    public void Start()
    {
        skills_slots = GetComponentsInChildren<Slot>();
    }

    public void AddSkill(Skill skill)
    {
        for (int i=0; i < skills_slots.Length; i++)
        {
            if (skills_slots[i].skill_in_panel == null)
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
