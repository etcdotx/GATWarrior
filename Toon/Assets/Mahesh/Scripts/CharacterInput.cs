using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
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
    public bool combatMode;

    public AudioSource lightAttack;
    public AudioSource heavyAttack;

    public GameObject fakeWeapon;
    public GameObject trueWeapon;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speedMultiplier = 1;

        combatMode = false;
        isRolling = false;

        fakeWeapon.SetActive(true);
        trueWeapon.SetActive(false);
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

        Roll();        
        DrawOrSheathe();
        Attack();
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

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !isRolling)
        {
            anim.SetTrigger("roll");
        }
    }

    void DrawOrSheathe()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) && !combatMode)
        {
            anim.SetBool("combatMode", true);
            StartCoroutine(ChangeMode());
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && combatMode)
        {
            anim.SetBool("combatMode", false);
            StartCoroutine(ChangeMode());
        }
    }

    void Attack() {
        if (combatMode)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                anim.SetTrigger("lightAttack");
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                anim.SetTrigger("heavyAttack");
            }
        }
    }

    IEnumerator ChangeMode() {
        yield return new WaitForSeconds(0.5f);
        if (combatMode)
            combatMode = false;
        else
            combatMode = true;
    }

    void ResetTrigger()
    {
        anim.ResetTrigger("lightAttack");
        anim.ResetTrigger("heavyAttack");
    }

    void LightAttack()
    {
        lightAttack.PlayOneShot(lightAttack.clip);
    }

    void HeavyAttack()
    {
        heavyAttack.PlayOneShot(heavyAttack.clip);
    }

    void DrawWeapon() {
        fakeWeapon.SetActive(false);
        trueWeapon.SetActive(true);
    }

    void SheatheWeapon()
    {
        fakeWeapon.SetActive(true);
        trueWeapon.SetActive(false);
    }
}
