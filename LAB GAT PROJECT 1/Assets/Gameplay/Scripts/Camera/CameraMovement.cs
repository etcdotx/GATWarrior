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

    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraOffsetZ;

    public float charOffsetX;
    public float charOffsetY;
    public float charOffsetZ;
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
        if (GameStatus.isTalking == false)
        {
            FollowCharacter();
        }
    }

    void FollowCharacter()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ) + rotation * dir;
        camTransform.LookAt(lookAt.position + new Vector3(charOffsetX, charOffsetY, charOffsetZ));
    }
}
