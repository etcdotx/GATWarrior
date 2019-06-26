using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementTest : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 inputAxis;
    public Animator anim;
    public float angle;
    public Quaternion targetRotation;
    public CameraMovement cameraMovement;
    public float rotateSpeed;
    public float rotateDamp;

    public float speedMultiplier;
    public float speedMultiplierLerp;
    public bool isRolling;

    private void Awake()
    {
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speedMultiplier = 1;
        isRolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputAxis();

        if (cameraMovement.isLocking)
        {
            Vector3 lookTarget = cameraMovement.monsterTarget.transform.position;
            lookTarget.y = transform.position.y;
            transform.LookAt(lookTarget);
        }
        else if (Mathf.Abs(inputAxis.x) > 0 || Mathf.Abs(inputAxis.y) > 0 && !isRolling)
        {
            if (Input.GetAxis("RT Button") > 0)
            {
                speedMultiplier = 2;
                Mathf.Lerp(speedMultiplier, 2, Time.deltaTime * speedMultiplierLerp);
            }
            else
            {
                speedMultiplier = 1;
            }
            Rotate();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !isRolling)
        {
            //CalculateDirection();
            anim.SetTrigger("roll");
        }
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxis("LeftJoystickHorizontal");
        inputAxis.y = Input.GetAxis("LeftJoystickVertical"); 

        anim.SetFloat("floatX", inputAxis.x * speedMultiplier);
        anim.SetFloat("floatY", inputAxis.y * speedMultiplier);
    }

    void CalculateDirection()
    {        
        angle = Mathf.Atan2(inputAxis.x, inputAxis.y);
        angle = Mathf.Rad2Deg * angle;
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y+rotateDamp, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
