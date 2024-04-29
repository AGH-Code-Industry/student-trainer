using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkill : MonoBehaviour
{
    public bool ifAddToSlot;
    public Skill skillToAdd;
    public SkillsPanelMenager skillsPanel;

    public void addSkill()
    {
        skillsPanel.AddSkill(skillToAdd);
        ifAddToSlot = false;
    }

    private void Update()
    {
        if (ifAddToSlot == true)
        {
            addSkill();
        }
    }
}