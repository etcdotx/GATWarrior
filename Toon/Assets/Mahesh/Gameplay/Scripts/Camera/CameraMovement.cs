using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public Transform camTransform;

    public float currentX;
    public float currentY;
    public float distance;
    public bool resetCurrent;

    public float cameraOffsetX;
    public float cameraOffsetY;
    public float cameraOffsetZ;

    public float charOffsetX;
    public float charOffsetY;
    public float charOffsetZ;

    public float camSensitivityX;
    public float camSensitivityY;
    public Vector3 inputAxis;

    [Header("Camera Lock")]
    public bool isLocking;
    public float cameraLockX;
    public float cameraLockY;
    public float cameraLockZ;
    public float cameraLockSpeed;

    public int monsterNum;
    public bool getMonster;
    public Vector3 lookVector;
    public GameObject monsterTarget;
    public GameObject[] nearestMonster;

    public Vector3 resetVector;
    public float test;

    [Header("Camera Collision")]
    public float minDistance;
    public float maxDistance;
    public float smooth;

    public float maxRayDistance;
    public Vector3 dollyDirAdjusted;
    public float colDis;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        resetCurrent = true;
        ApplyMovement();
    }

    private void Update()
    {
        GetInputAxis();
        if (Mathf.Abs(inputAxis.x) > 0.15 || Mathf.Abs(inputAxis.y) > 0.15 && isLocking==false) {
            if (GameStatus.isTalking == false && GameStatus.CanMove == true)
            {
                if (currentY <= 80 || currentY >= -80)
                {
                    currentY += Input.GetAxis("RightJoystickVertical") * camSensitivityY;
                }
                if (currentY >= 80)
                {
                    currentY = 79.9f;
                }
                if (currentY <= -80)
                {
                    currentY = -79.9f;
                }
                currentX += Input.GetAxis("RightJoystickHorizontal") * camSensitivityX;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameStatus.isTalking == false && GameStatus.CanMove == true)
        {
            if (Input.GetAxisRaw("LT Button") == 1)
            {
                CameraLock();
                resetCurrent = true;
            }
            else
            {
                getMonster = false;
                isLocking = false;
                ApplyMovement();
            }
        }
    }


    void GetInputAxis() {
        inputAxis.x = Input.GetAxis("RightJoystickHorizontal");
        inputAxis.y = Input.GetAxis("RightJoystickVertical");
    }

    void ApplyMovement()
    {
        if (resetCurrent == true)
        {
            currentX = player.transform.eulerAngles.y;
            currentY = 0;
            resetCurrent = false;
        }

        maxRayDistance = Vector3.Distance(transform.position, player.transform.position);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.forward, Color.green);

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            //Debug.Log("hit = "+ hit.collider.name);
            //colDis = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            //transform.position = Vector3.Lerp(transform.position, player.transform.position * colDis, Time.deltaTime * smooth);
        }
        //else {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            Vector3 targetPos = player.transform.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ) + rotation * dir;
            transform.position = targetPos;
        //}
        transform.LookAt(player.transform.position + new Vector3(charOffsetX, charOffsetY, charOffsetZ));
    }

    void CameraLock()
    {
        if (getMonster == false)
        {
            monsterTarget = null;
            monsterNum = 0;
            nearestMonster = GameObject.FindGameObjectsWithTag("Monster");
            getMonster = true;
        }
        if (nearestMonster.Length != 0)
        {
            isLocking = true;
            for (int i = 0; i < nearestMonster.Length; i++)
            {
                if (nearestMonster[i].GetComponent<MonsterStatus>().hp <= 0)
                {
                    nearestMonster = GameObject.FindGameObjectsWithTag("Monster");
                    monsterNum = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                monsterNum++;
                if (monsterNum > nearestMonster.Length - 1)
                {
                    monsterNum = 0;
                }
            }

            monsterTarget = nearestMonster[monsterNum];
            transform.LookAt(monsterTarget.transform.position);
            lookVector = player.transform.position - monsterTarget.transform.position;

            lookVector = Vector3.ClampMagnitude(lookVector, 0.4f);

            if (lookVector.sqrMagnitude < 0.4f * 0.4f)
                lookVector = lookVector.normalized * 0.4f;
            
            Vector3 getPos = player.transform.position + lookVector + transform.right * cameraLockX + transform.up * cameraLockY + transform.forward * cameraLockZ;
            transform.position = getPos;
        }
        else {
            getMonster = false;
            ApplyMovement();
        }
    }

}
