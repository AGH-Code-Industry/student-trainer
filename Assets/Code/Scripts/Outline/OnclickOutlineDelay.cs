using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnclickOutlineDelay : MonoBehaviour
{

    private HighlightManager hmScript;
    private Outline _outline;
    // Start is called before the first frame update
    void Start()
    {
        hmScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HighlightManager>();
        _outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    hmScript.SelectedHighlight();
                    StartCoroutine(CallDeselect());
                }
            }
        }
    }

    IEnumerator CallDeselect()
    {
        yield return new WaitForSeconds(5f);
        hmScript.DeselectHighlight();
    }
}
