using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillInSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skillInPanel;

    public Image icon;
    public Image background;

    public Color defaultBackgroundColor = Color.white;
    public Color hoverBackgroundColor = Color.blue;

    //public Sprite defaultBackgroundSprite;
    //public Sprite hoverBackgroundSprite;

    public void Start()
    {
        //background.sprite = defaultBackgroundSprite;
        background.color = defaultBackgroundColor;
    }

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

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        if (background != null && hoverBackgroundSprite != null)
        {
            background.sprite = hoverBackgroundSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (background != null && defaultBackgroundSprite != null)
        {
            background.sprite = defaultBackgroundSprite;
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (background != null)
        {
            background.color = hoverBackgroundColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (background != null)
        {
            background.color = defaultBackgroundColor;
        }
    }

}