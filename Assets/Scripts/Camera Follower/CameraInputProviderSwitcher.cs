using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInputProviderSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FreeCameraCin;

    // Update is called once per frame
    void Update()
    {
         if (Input.GetMouseButton(1))
                        {
                            FreeCameraCin.GetComponent<CinemachineInputProvider>().enabled = true;
                        }
                        
                        if (Input.GetMouseButtonUp(1))
                        {
                            FreeCameraCin.GetComponent<CinemachineInputProvider>().enabled = false;
                        }
    }
}
