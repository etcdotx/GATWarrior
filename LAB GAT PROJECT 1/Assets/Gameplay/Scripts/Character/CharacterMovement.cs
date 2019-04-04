using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public InputSetup inputSetup;
    public CharacterAttack characterAttack;
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
        GameStatus.ResumeMove();
        characterAttack = GetComponent<CharacterAttack>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        charRig = gameObject.GetComponent<Rigidbody>();
        try
        {
            charAnim = GetComponent<Animator>();
        } catch
        {
            Debug.Log("No Animator found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus.isTalking == false && InputHolder.isInputHolded == false 
            && GameStatus.CanMove == true && characterAttack.isAttacking == false)//
        {
            GetInputAxis();
            if (Mathf.Abs(inputAxis.x) > 0.15 || Mathf.Abs(inputAxis.y) > 0.15)
            {
                CalculateDirection();
                Rotate();
                Walk();
                ManageSpeed();
            }
            else
            {
                try
                {
                    charAnim.SetBool("isWalk", false);
                }
                catch { }
            }
            if (GameStatus.IsPaused == false)
            {
                Jump();
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
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void ManageSpeed() {
        if (inputAxis.z == 1)
        {
            currentSpeed = defaultSpeed * runSpeedMultiplier;
        }
        else
        {
            currentSpeed = defaultSpeed;
        }
    }

    void Walk()
    {

        if (Input.GetAxis("LeftJoystickVertical") > 0)
        {
            if (Input.GetAxis("LeftJoystickHorizontal") > 0)
            {
                ApplyMovement(1, 1);
            }
            else
            {
                ApplyMovement(1, -1);
            }
        }
        else
        {
            if (Input.GetAxis("LeftJoystickHorizontal") > 0)
            {
                ApplyMovement(-1, 1);
            }
            else
            {
                ApplyMovement(-1, -1);
            }
        }
    }

    void ApplyMovement(int a, int b) {
        Vector3 movedirection = a * transform.forward * Input.GetAxis("LeftJoystickVertical") + b * transform.forward * Input.GetAxis("LeftJoystickHorizontal");
        transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
        try
        {
            charAnim.SetBool("isWalk", true);
        }
        catch { }
    }

    void Jump()
    {
        if (Input.GetKeyDown(inputSetup.jump))
        {
            charRig.velocity += new Vector3(0, jumpForce, 0);
        }
    }
}
