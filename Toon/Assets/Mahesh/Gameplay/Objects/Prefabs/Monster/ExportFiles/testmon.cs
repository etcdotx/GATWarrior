using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmon : MonoBehaviour
{
    Animator anim;

    public float totalserang;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("mundur");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("mental");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetTrigger("serang");
            totalserang = 3;
            //totalserang = Random.Range()
        }
    }

    void checkSerang() {
        if (totalserang != 0)
        {
            anim.SetTrigger("serang");
            totalserang--;
        }
    }
}
