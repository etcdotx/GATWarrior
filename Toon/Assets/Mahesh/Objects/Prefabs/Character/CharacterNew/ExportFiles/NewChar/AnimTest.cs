using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("attack1");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("attack2");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("attack3");
        }
    }
}
