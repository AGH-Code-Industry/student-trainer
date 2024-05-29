using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotManager : MonoBehaviour
{
    private List<Renderer> children;
    private Color originalColor;
    private Mouse mouse;
    private Transform selectedItem;
    private Vector3 rect;
    private Renderer highlightedChild;
    public TextMeshPro text;
    void Start()
    {
        mouse = FindObjectOfType<Mouse>();
        children = new List<Renderer>();

        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                children.Add(renderer);
            }
        }
        if (children.Count > 0)
        {
            rect = new Vector3(children[0].transform.localScale.x, children[0].transform.localScale.y, 0);
            originalColor = children[0].material.color;
        }
    }

    void Update()
    {
        highlightedChild = null;

        foreach (Renderer child in children)
        {
            Transform childTransform = child.GetComponent<Transform>();
            if (mouse.IsOver(childTransform, mouse.position, rect))
            {
                HighlightChild(child);
                highlightedChild = child;
                if (childTransform.childCount > 0)
                {
                    text.text = childTransform.GetChild(0).GetComponent<Item>().itemDescription;
                }
                else if (selectedItem == null)
                {
                    text.text = "";
                }
            }
            else
            {
                UnhighlightChild(child);
            }
        }

        if (selectedItem != null)
        {
            selectedItem.position = new Vector3(mouse.position.x, mouse.position.y, selectedItem.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    void HighlightChild(Renderer child)
    {
        Color highlightColor = originalColor + new Color(0.1f, 0.1f, 0.1f);
        child.material.color = highlightColor;
    }

    void UnhighlightChild(Renderer child)
    {
        child.material.color = originalColor;
    }

    void HandleMouseClick()
    {
        if (highlightedChild != null)
        {
            Transform childTransform = highlightedChild.transform;
            if (selectedItem == null)
            {
                if (childTransform.childCount > 0)
                {
                    selectedItem = childTransform.GetChild(0);
                }
            }
            else if (childTransform.childCount == 0 || childTransform == selectedItem.parent)
            {
                selectedItem.SetParent(childTransform);
                selectedItem.localPosition = Vector3.zero;
                selectedItem = null;
            }
            else if (childTransform.childCount != 0)
            {
                Transform item = childTransform.GetChild(0);
                item.SetParent(selectedItem.parent);
                selectedItem.SetParent(childTransform);
                selectedItem.localPosition = Vector3.zero;
                selectedItem = item;
            }
        }
    }
}
