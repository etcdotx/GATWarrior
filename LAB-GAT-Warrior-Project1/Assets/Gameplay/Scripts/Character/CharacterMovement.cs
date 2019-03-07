using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject MainCamera;

    public float DefaultSpeed;
    public float RunSpeedMultiplier;
    public float Speed;
    public float RotateSpeed;

    public Vector3 InputAxis;
    public Quaternion TargetRotation;
    public float Angle;

    // Use this for initialization
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        GetInputAxis();
        if (InputAxis.x >= 0.1 || InputAxis.y >= 0.1 || InputAxis.x <= -0.1 || InputAxis.y <= -0.1)
        {
            CalculateDirection();
            Rotate();
            walk();

            if (InputAxis.z == 1)
            {
                Speed = DefaultSpeed * RunSpeedMultiplier;
            }
            else
            {
                Speed = DefaultSpeed;
            }
            InputAxis = Vector3.zero;
        }
    }

    void GetInputAxis()
    {
        InputAxis.x = Input.GetAxis("LeftJoystickHorizontal");
        InputAxis.y = Input.GetAxis("LeftJoystickVertical");

        //run
        if (Input.GetAxis("RT Button")==1 || Input.GetKey(KeyCode.LeftShift))
        {
            InputAxis.z = 1f;
        }
        else
        {
            InputAxis.z = 0f;
        }
    }

    

    void CalculateDirection()
    {
        Angle = Mathf.Atan2(InputAxis.x, InputAxis.y);
        
        Angle = Mathf.Rad2Deg * Angle;
        
    }

    void Rotate()
    {
        TargetRotation = Quaternion.Euler(0, Angle+MainCamera.transform.eulerAngles.y, 0);
        //Debug.Log(MainCamera.transform.eulerAngles.y);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, TargetRotation, RotateSpeed * Time.deltaTime);
    }

    void walk()
    {
        Vector3 movedirection = MainCamera.transform.forward * Input.GetAxis("LeftJoystickVertical") + MainCamera.transform.right * Input.GetAxis("LeftJoystickHorizontal");
        transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * Speed;
    }
}
