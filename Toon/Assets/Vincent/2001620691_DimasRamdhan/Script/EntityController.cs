using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    public Animator Anim;
    public KeyCode Action;

    // Use this for initialization
    void Start () {

      
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            gameObject.GetComponent<Animator>().SetBool("toClap", true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            gameObject.GetComponent<Animator>().SetBool("toKick", true);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            gameObject.GetComponent<Animator>().SetBool("toThinking", true);
    }
}
