using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLift : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            animator.SetTrigger("move");
        }
    }
}
