using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyTempUnkillable : MonoBehaviour, IDamageable
{
    public Renderer[] renderers;

    public Material normalMat, damageMat;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        foreach(Renderer r in renderers)
        {
            r.material = damageMat;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (Renderer r in renderers)
        {
            r.material = normalMat;
        }
    }
}