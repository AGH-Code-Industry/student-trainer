using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkill : MonoBehaviour
{
    public bool if_add_to_slot;
    public Skill skill_to_add;
    public Skills_panel_Menager skills_panel;

    public void add_skill()
    {
        skills_panel.AddSkill(skill_to_add);
        if_add_to_slot = false;
    }

    private void Update()
    {
        if (if_add_to_slot == true)
        {
            add_skill();
        }
    }
}
