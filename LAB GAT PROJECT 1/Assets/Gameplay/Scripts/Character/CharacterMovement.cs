using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject mainCamera;

    public float defaultSpeed;
    public float runSpeedMultiplier;
    public float currentSpeed;
    public float rotateSpeed;

    public Vector3 inputAxis;
    public Quaternion targetRotation;
    public float angle;
    public Animator charAnim;

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        try
        {
            charAnim = gameObject.GetComponent<Animator>();
        } catch
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        getInputAxis();
        if (inputAxis.x >= 0.1 || inputAxis.y >= 0.1 || inputAxis.x <= -0.1 || inputAxis.y <= -0.1)
        {
            calculateDirection();
            rotate();
            walk();

            if (inputAxis.z == 1)
            {
                currentSpeed = defaultSpeed * runSpeedMultiplier;
            }
            else
            {
                currentSpeed = defaultSpeed;
            }
            inputAxis = Vector3.zero;
        }
        else
        {
            try
            {
                charAnim.SetBool("isWalk", false);
            } catch { }
        }
    }

    void getInputAxis()
    {
        inputAxis.x = Input.GetAxis("LeftJoystickHorizontal");
        inputAxis.y = Input.GetAxis("LeftJoystickVertical");

        //run
        if (Input.GetAxis("RT Button")==1 || Input.GetKey(KeyCode.LeftShift))
        {
            inputAxis.z = 1f;
        }
        else
        {
            inputAxis.z = 0f;
        }
    }

    

    void calculateDirection()
    {
        angle = Mathf.Atan2(inputAxis.x, inputAxis.y);
        
        angle = Mathf.Rad2Deg * angle;
        
    }

    void rotate()
    {
        targetRotation = Quaternion.Euler(0, angle+ mainCamera.transform.eulerAngles.y, 0);
        //Debug.Log(MainCamera.transform.eulerAngles.y);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void walk()
    {
        Debug.Log(transform.forward * Input.GetAxis("LeftJoystickVertical"));
        Debug.Log(mainCamera.transform.right * Input.GetAxis("LeftJoystickHorizontal"));

        if (Input.GetAxis("LeftJoystickVertical") > 0)
        {
            if (Input.GetAxis("LeftJoystickHorizontal") > 0)
            {
                Vector3 movedirection = transform.forward * Input.GetAxis("LeftJoystickVertical") + transform.forward * Input.GetAxis("LeftJoystickHorizontal");
                transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
                try
                {
                    charAnim.SetBool("isWalk", true);
                }
                catch { }
            }
            else {
                Vector3 movedirection = transform.forward * Input.GetAxis("LeftJoystickVertical") -transform.forward * Input.GetAxis("LeftJoystickHorizontal");
                transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
                try
                {
                    charAnim.SetBool("isWalk", true);
                }
                catch { }
            }
        }
        else
        {
            if (Input.GetAxis("LeftJoystickHorizontal") > 0)
            {
                Vector3 movedirection = -transform.forward * Input.GetAxis("LeftJoystickVertical") + transform.forward * Input.GetAxis("LeftJoystickHorizontal");
                transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
                try
                {
                    charAnim.SetBool("isWalk", true);
                }
                catch { }
            }
            else {
                Vector3 movedirection = -transform.forward * Input.GetAxis("LeftJoystickVertical") - transform.forward * Input.GetAxis("LeftJoystickHorizontal");
                transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
                try
                {
                    charAnim.SetBool("isWalk", true);
                }
                catch { }
            }
        }
    }
}
