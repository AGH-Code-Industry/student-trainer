using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyTemp : MonoBehaviour, IDamageable
{
    public Renderer[] renderers;

    float health = 55;

    public Material normalMat, damageMat;
    public GameObject deathParticles;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Die();
            return;
        }

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

    void Die()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}