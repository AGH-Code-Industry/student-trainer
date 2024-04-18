using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutlineOnStart : MonoBehaviour
{

    private float delay = 0.01f;
  
    private Outline _outline;
    // Start is called before the first frame update
    void Start()
    {
        _outline = GetComponent<Outline>();
        Invoke("DisableOutline", delay);
    }

    private void DisableOutline()
    {
        _outline.enabled = false;
    }
   
}
