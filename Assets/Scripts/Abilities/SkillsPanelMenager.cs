using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanelMenager : MonoBehaviour
{
    public SkillInSlot[] skillsSlots;
    public List<Skill> skills = new List<Skill>();

    public void Awake()
    {
        skillsSlots = GetComponentsInChildren<SkillInSlot>();
    }

    public void Start()
    {
        foreach (Skill skill in skills)
        {
            AddSkill(skill);
        }
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
