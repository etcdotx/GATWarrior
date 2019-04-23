using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public Animator anim;
    public float maxHealth;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = 100;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaged(float dmg) {
        health -= dmg;
        anim.SetTrigger("attacked");
    }
}
