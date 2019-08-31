using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public static CharacterInput instance;

    public Camera mainCamera;
    public Vector3 inputAxis;
    public Animator animator;
    public float angle;
    public Quaternion targetRotation;
    public CameraMovement cameraMovement;
    public float rotateSpeed;
    public float rotateDamp;

    public float speedMultiplier;
    public float speedMultiplierLerp;
    public bool isRolling;
    public bool combatMode;
    public bool turningBack;

    public float rollCooldown;

    public AudioSource lightAttack;
    public AudioSource heavyAttack;

    public GameObject fakeWeapon;
    public GameObject trueWeapon;

    [Header("usableItem")]
    public Item selectedItem;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        animator = GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
    }

    void Start()
    {
        speedMultiplier = 1;

        combatMode = false;
        isRolling = false;
        turningBack = false;

        fakeWeapon.SetActive(true);
        trueWeapon.SetActive(false);
    }

    void Update()
    {
        if (UIManager.instance.uiState == UIState.Gameplay || UIManager.instance.uiState == UIState.InventoryAndSave)
        {
            GetInputAxis();

            if (cameraMovement.isLocking)
            {
                Vector3 lookTarget = cameraMovement.monsterTarget.transform.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget);
            }

            if (Mathf.Abs(inputAxis.x) > 0 || Mathf.Abs(inputAxis.y) > 0 && !isRolling && !turningBack)
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

                if (!CharacterInteraction.instance.isGathering && !cameraMovement.isLocking)
                {
                    Rotate();
                }
            }

            if (UIManager.instance.uiState == UIState.Gameplay) //kalau lagi ga savestate
            {
                Roll();
                DrawOrSheathe();
                Attack();
                Block();

                if (!cameraMovement.isLocking)
                    TurnBack();
            }
        }
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxis("LeftJoystickHorizontal");
        inputAxis.y = Input.GetAxis("LeftJoystickVertical");

        animator.SetFloat("floatX", inputAxis.x * speedMultiplier);
        animator.SetFloat("floatY", inputAxis.y * speedMultiplier);

        if (Mathf.Abs(animator.GetFloat("floatY")) >= Mathf.Abs(animator.GetFloat("floatX")))
            animator.SetFloat("movementSpeed", Mathf.Abs(animator.GetFloat("floatY")));
        else
            animator.SetFloat("movementSpeed", Mathf.Abs(animator.GetFloat("floatX")));
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(inputAxis.x, inputAxis.y);
        angle = Mathf.Rad2Deg * angle;
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y + rotateDamp, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void TurnBack() {
        if (Input.GetAxis("D-Pad Down") > 0 && !turningBack)
        {
            StopCoroutine(TurningBack());
            turningBack = true;
            StartCoroutine(TurningBack());
        }
    }

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !isRolling && rollCooldown==0)
        {
            animator.SetTrigger("roll");
            rollCooldown = 1;
            StartCoroutine(RollCooldown());
        }
    }

    IEnumerator RollCooldown() {
        yield return new WaitForSeconds(0.75f);
        rollCooldown = 0;
    }

    void DrawOrSheathe()
    {
        if (Input.GetKeyDown(InputSetup.instance.attack) && !combatMode)
        {
            animator.SetTrigger("changeCombatMode");
        }
        else if (Input.GetKeyDown(InputSetup.instance.sheatheWeapon) && combatMode)
        {
            animator.SetTrigger("changeCombatMode");
        }
    }

    void Attack() {
        if (combatMode)
        {
            if (Input.GetKeyDown(InputSetup.instance.attack))
            {
                animator.SetTrigger("lightAttack");
            }
            if (Input.GetKeyDown(InputSetup.instance.attack2))
            {
                animator.SetTrigger("heavyAttack");
            }
        }
    }

    void Block()
    {
        if (Input.GetKey(InputSetup.instance.block))
        {
            animator.SetBool("block", true);
        }
        else if (Input.GetKeyUp(InputSetup.instance.block))
        {
            animator.SetBool("block", false);
        }
    }

    void ChangeMode() {
        if (combatMode)
            combatMode = false;
        else
            combatMode = true;

        animator.SetBool("combatMode", combatMode);
        CameraMovement.instance.ResetCamera();
    }

    IEnumerator TurningBack()
    {
        cameraMovement.cameraTurnBack.gameObject.SetActive(true);
        if (animator.GetFloat("floatY") > 1)
            animator.SetFloat("floatTurn", 1);
        else
            animator.SetFloat("floatTurn", 0);

        animator.SetTrigger("turn180");
        yield return new WaitForSeconds(1f);
        cameraMovement.cameraTurnBack.gameObject.SetActive(false);
        turningBack = false;
    }

    void ResetTrigger()
    {
        animator.ResetTrigger("lightAttack");
        animator.ResetTrigger("heavyAttack");
        animator.ResetTrigger("changeCombatMode");
        animator.ResetTrigger("drink");
        animator.ResetTrigger("roll");
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

    public void useItem ()
    {
        selectedItem.Use();
        for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
        {
            Inventory.instance.inventoryIndicator[i].GetComponent<InventoryIndicator>().Refresh();
        }
        Inventory.instance.RefreshInventory();
    }
}
