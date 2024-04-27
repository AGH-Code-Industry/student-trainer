using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public panel_abilities panel_abilities;

    public Image background;

    public string skillName;

    void Start()
    {
        background = GetComponent<Image>();
        panel_abilities.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        panel_abilities.OnTabSelected(this);
        panel_abilities.OnSkillButtonClicked(skillName);
        Debug.Log(skillName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panel_abilities != null)
        {
            panel_abilities.OnTabEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel_abilities.OnTabExit(this);
    }
}
