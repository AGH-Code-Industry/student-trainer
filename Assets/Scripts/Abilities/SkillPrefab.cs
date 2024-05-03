using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInSlot : MonoBehaviour, IPointerClickHandler
{
    public Skill skillInPanel;

    public Image icon;
    public void AddSkillToSlot(Skill skill)
    {
        skillInPanel = skill;
        icon.sprite = skill.icon;
        icon.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (skillInPanel != null)
        {
            Abilities.Instance.PerformSkill(skillInPanel.name);
        }
    }

}