using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform lookAt;
    public Transform camTransform;

    public float currentX;
    public float currentY;
    public float distance;

    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraOffsetZ;

    public float charOffsetX;
    public float charOffsetY;
    public float charOffsetZ;

    private void Update()
    {
        if (currentY <= 80 || currentY >= -80) {
            currentY += Input.GetAxis("RightJoystickVertical");
        }
        if (currentY >= 80)
        {
            currentY = 79.9f;
        }
        if (currentY <= -80)
        {
            currentY = -79.9f;
        }
        currentX += Input.GetAxis("RightJoystickHorizontal");       
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (GameStatus.isTalking == false)
        {
            ApplyMovement();
        }
    }

    void ApplyMovement()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ) + rotation * dir;
        transform.LookAt(lookAt.position + new Vector3(charOffsetX, charOffsetY, charOffsetZ));
    }
}
