using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {
    public Camera m_main;
    public GameObject m_camera;

    void Start() {
        m_main = Camera.main;
        m_camera = m_main.gameObject;
    }

    void Update() {
        m_camera.transform.rotation *= Quaternion.Euler( 0, 10f * Time.fixedDeltaTime, 0);
    }

}
