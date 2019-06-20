using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public Transform camTransform;

    public CinemachineTargetGroup ctg;
    public GameObject thirdVPersonCamera;
    public GameObject lockVCamera;

    [Header("Camera Lock")]
    public bool isLocking;
    public int monsterNum;
    public bool getMonster;
    public GameObject monsterTarget;
    public GameObject[] nearestMonster;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        thirdVPersonCamera.SetActive(true);
        lockVCamera.SetActive(false);
    }

    private void Update()
    {
        if (GameStatus.isTalking == false && GameStatus.CanMove == true)
        {
            if (Input.GetAxisRaw("LT Button") == 1)
            {
                CameraLock();
            }
            else
            {
                getMonster = false;
                isLocking = false;
                thirdVPersonCamera.SetActive(true);
                lockVCamera.SetActive(false);
            }
        }
    }

    void CameraLock()
    {
        if (getMonster == false)
        {
            monsterTarget = null;
            monsterNum = 0;
            nearestMonster = GameObject.FindGameObjectsWithTag("Monster");
            getMonster = true;
            thirdVPersonCamera.SetActive(false);
            lockVCamera.SetActive(true);
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

            ctg.m_Targets[1].target = nearestMonster[monsterNum].transform;
        }
        else {
            getMonster = false;
            isLocking = false;
            thirdVPersonCamera.SetActive(true);
            lockVCamera.SetActive(false);
        }
    }

}
