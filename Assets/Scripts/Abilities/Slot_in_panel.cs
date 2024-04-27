using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Skill skill_in_panel;

    public Image ikona_skilla;
    public void AddSkillToSlot(Skill skill)
    {
        skill_in_panel = skill;
        ikona_skilla.sprite = skill.icon;
        ikona_skilla.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(skill_in_panel.skillname);
    }
}
