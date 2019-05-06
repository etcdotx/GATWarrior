using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
    }

    void FixedUpdate() {
        this.gameObject.transform.rotation *= Quaternion.Euler(1 * Time.fixedDeltaTime, 1 * Time.fixedDeltaTime, 1 * Time.fixedDeltaTime);
    }
}
