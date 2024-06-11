using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Image target;

    public Color32 defaultColor;
    public Color32 enterColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("klik");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        target.color = enterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target.color = defaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
