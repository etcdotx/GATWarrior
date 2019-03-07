using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Camera cam;
    public Transform lookAt;
    public Transform camTransform;

    public float currentX;
    public float currentY;
    public float distance;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("RightJoystickHorizontal");
        currentY += Input.GetAxis("RightJoystickVertical");
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
