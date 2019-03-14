using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public InputSetup inputSetup;
    public Rigidbody charRig;

    public GameObject mainCamera;

    public float defaultSpeed;
    public float runSpeedMultiplier;
    public float currentSpeed;
    public float rotateSpeed;
    public float jumpForce;

    public Vector3 inputAxis;
    public Quaternion targetRotation;
    public float angle;
    public Animator charAnim;

    // Use this for initialization
    void Start()
    {
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        charRig = gameObject.GetComponent<Rigidbody>();
        try
        {
            charAnim = gameObject.GetComponent<Animator>();
        } catch
        {
            Debug.Log("No Animator found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.isTalking == false && GameStatus.IsPaused == false && InputHolder.isInputHolded == false)
        {
            GetInputAxis();
            if (inputAxis.x >= 0.1 || inputAxis.y >= 0.1 || inputAxis.x <= -0.1 || inputAxis.y <= -0.1)
            {
                CalculateDirection();
                Rotate();
                Walk();

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
                }
                catch { }
            }

            if (Input.GetKeyDown(inputSetup.jump))
            {
                charRig.velocity += new Vector3(0, jumpForce, 0);
            }
        }
    }

    void GetInputAxis()
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

    void CalculateDirection()
    {
        angle = Mathf.Atan2(inputAxis.x, inputAxis.y);
        angle = Mathf.Rad2Deg * angle;
        
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle+ mainCamera.transform.eulerAngles.y, 0);
        //Debug.Log(MainCamera.transform.eulerAngles.y);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void Walk()
    {

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
