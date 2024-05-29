using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAnimation : MonoBehaviour
{
    private npcMovement npcMovement;
    public Animator animator;
    void Start()
    {
        npcMovement = GetComponent<npcMovement>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetBool("isMoving", npcMovement.isMoving);
    }
}
