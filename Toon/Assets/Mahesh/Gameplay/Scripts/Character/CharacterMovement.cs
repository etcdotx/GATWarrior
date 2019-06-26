using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public InputSetup inputSetup;
    public CharacterCombat characterCombat;
    public Rigidbody charRig;

    public GameObject mainCamera;
    public CameraMovement cameraMovement;

    public float rotateSpeed;
    public float lockRotationSpeed;
    public bool lockWalk;

    public Vector3 inputAxis;
    public Quaternion targetRotation;
    public float angle;
    public Animator charAnim;
    
    [Header("Movement Speed")]
    public float maxDefaultSpeed;
    public float maxRunSpeed;
    public float currentSpeed;
    public float accelerateRatePerSec;
    public float defaultTimeZeroToMax=1f;
    public float runTimeZeroToMax = 2.5f;

    [Header("Movement Logic")]
    public float curAng;
    public Vector2 curVecMovement;
    public Vector2 overVecMovement;

    public float test;
    public bool canMove;
    public bool holdRoll;

    // Use this for initialization
    void Start()
    {
        GameStatus.ResumeMove();
        characterCombat = GetComponent<CharacterCombat>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMovement = mainCamera.GetComponent<CameraMovement>();

        charRig = gameObject.GetComponent<Rigidbody>();

	holdRoll = false;
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
        if (!InputHolder.isInputHolded && GameStatus.CanMove && !characterCombat.isAttacking && canMove)//
        {
            GetInputAxis();
            if (characterCombat.isShielding == true && cameraMovement.isLocking == true && cameraMovement.monsterTarget!=null)
            {
                Vector3 lookTarget = cameraMovement.monsterTarget.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget);
            }
            if (Mathf.Abs(inputAxis.x) > 0 || Mathf.Abs(inputAxis.y) > 0)
            {
                ManageSpeed();
                if (characterCombat.isShielding == true && cameraMovement.isLocking == true)
                {
                    lockWalk = true;
                }
                else
                {
                    lockWalk = false;
                    CheckMoveToRotation();
                    CalculateDirection();
                    Rotate();
                }
                Walk();
            }
            else
            {
                try
                {
                    charAnim.SetBool("isWalk", false);
                    currentSpeed = 0;
                }
                catch { }
            }
            if (GameStatus.IsPaused == false)
            {
                Roll();
            }
        }
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxis("LeftJoystickHorizontal");
        inputAxis.y = Input.GetAxis("LeftJoystickVertical");

        //run
        if (characterCombat.isShielding == false)
        {
            if (Input.GetAxisRaw("RT Button") == 1)
            {
                inputAxis.z = 1f;
            }
            else
            {
                inputAxis.z = 0f;
            }
        }
    }

    void CheckMoveToRotation() {
        float angle1 = Mathf.Atan2(overVecMovement.x, overVecMovement.y);
        float angle2 = Mathf.Atan2(curVecMovement.x, curVecMovement.y);

        overVecMovement = new Vector2(Mathf.Abs(inputAxis.x), Mathf.Abs(inputAxis.y));
        angle1 = Mathf.Rad2Deg * angle1;

        float curRotation = Mathf.Abs(angle1 - curAng);
        if (curRotation > 20)
        {
            currentSpeed = 0;
        }

        curVecMovement = new Vector2(Mathf.Abs(inputAxis.x), Mathf.Abs(inputAxis.y));
        angle2 = Mathf.Rad2Deg * angle2;
        curAng = angle2;
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(inputAxis.x, inputAxis.y);
        angle = Mathf.Rad2Deg * angle;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle + mainCamera.transform.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void ManageSpeed() {
        if (inputAxis.z == 1)
        {
            accelerateRatePerSec = maxRunSpeed / runTimeZeroToMax;
            currentSpeed += accelerateRatePerSec * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxRunSpeed);
            //currentSpeed = maxRunSpeed;
        }
        else
        {
            accelerateRatePerSec = maxDefaultSpeed / defaultTimeZeroToMax;
            currentSpeed += accelerateRatePerSec * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxDefaultSpeed);
            //currentSpeed = maxDefaultSpeed;
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

    void ApplyMovement(int a, int b)
    {
        Vector3 movedirection = new Vector3(0,0,0);
        if (lockWalk == false)
        {
            movedirection = a * transform.forward * Input.GetAxis("LeftJoystickVertical") + b * transform.forward * Input.GetAxis("LeftJoystickHorizontal");

        }
        else
        {
            Vector3 lookt = cameraMovement.nearestMonster[cameraMovement.monsterNum].transform.position;
            //lookt.y = 0;
            transform.LookAt(lookt);
            if (a > 0)
            {
                if (b > 0)
                    movedirection = a * transform.forward * Input.GetAxis("LeftJoystickVertical") + b * transform.right * Input.GetAxis("LeftJoystickHorizontal");
                else
                    movedirection = a * transform.forward * Input.GetAxis("LeftJoystickVertical") + -b * transform.right * Input.GetAxis("LeftJoystickHorizontal");
            }
            else {
                if (b > 0)
                    movedirection = -a * transform.forward * Input.GetAxis("LeftJoystickVertical") + b * transform.right * Input.GetAxis("LeftJoystickHorizontal");
                else
                    movedirection = -a * transform.forward * Input.GetAxis("LeftJoystickVertical") + -b * transform.right * Input.GetAxis("LeftJoystickHorizontal");
            }
        }
        transform.localPosition = transform.localPosition + movedirection * Time.deltaTime * currentSpeed;
        try
        {
            charAnim.SetBool("isWalk", true);
        }
        catch { }
    }

    void Roll()
    {
        if (Input.GetKeyDown(inputSetup.jump) && holdRoll == false)
        {
	        StartCoroutine(HoldRoll());
            charAnim.SetTrigger("roll");
        }
    }

    public IEnumerator HoldRoll() {
        holdRoll = true;
        yield return new WaitForSeconds(1);
        holdRoll = false;
    }

    public IEnumerator BlockFail() {
        canMove = false;
        yield return new WaitForSeconds(1);
        canMove = true;
    }
    
    public IEnumerator BlockSuccess()
    {
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }
}
