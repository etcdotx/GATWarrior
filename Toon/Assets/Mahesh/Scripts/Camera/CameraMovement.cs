using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public CinemachineTargetGroup ctg;
    public CinemachineFreeLook cameraNormal;
    public CinemachineFreeLook cameraFight;
    public CinemachineFreeLook cameraTurnBack;
    public CinemachineVirtualCamera cenemytarget;

    [Header("Camera Lock")]
    public bool isLocking;
    public int monsterNum;
    public bool getMonster;
    public Transform monsterTarget;
    public GameObject[] nearestMonster;
    public GameObject targetIndicator;

    public bool characterInCombat;
    public bool turningBack;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        cameraTurnBack.gameObject.SetActive(false);
        characterInCombat = false;

        cameraNormal.Priority = 3;
        cameraFight.Priority = 2;
        cenemytarget.Priority = 1;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("LT Button") == 1)
        {
            CameraLock();
        }
        else
        {
            targetIndicator.SetActive(false);
            getMonster = false;
            isLocking = false;

            if (characterInCombat)
            {
                cameraNormal.gameObject.SetActive(false);
                cameraFight.gameObject.SetActive(true);
                cenemytarget.gameObject.SetActive(false);

                //cameraNormal.Priority = 2;
                //cameraFight.Priority = 3;
                //cenemytarget.Priority = 1;
            }
            else
            {
                cameraNormal.gameObject.SetActive(true);
                cameraFight.gameObject.SetActive(false);
                cenemytarget.gameObject.SetActive(false);
                //cameraNormal.Priority = 3;
                //cameraFight.Priority = 2;
                //cenemytarget.Priority = 1;
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

            cameraNormal.gameObject.SetActive(false);
            cameraFight.gameObject.SetActive(false);
            cenemytarget.gameObject.SetActive(true);
        }
        if (nearestMonster.Length != 0)
        {
            targetIndicator.SetActive(true);
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

            //ctg.m_Targets[1].target = nearestMonster[monsterNum].transform;
            cenemytarget.LookAt = nearestMonster[monsterNum].transform;
            monsterTarget = nearestMonster[monsterNum].transform;

            Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(nearestMonster[monsterNum].transform.position);
            targetIndicator.transform.position = targetScreenPoint;
        }
        else {
            getMonster = false;
            isLocking = false;

            if (characterInCombat)
            {
                cameraNormal.gameObject.SetActive(false);
                cameraFight.gameObject.SetActive(true);
                cenemytarget.gameObject.SetActive(false);

                //cameraNormal.Priority = 2;
                //cameraFight.Priority = 3;
                //cenemytarget.Priority = 1;
            }
            else
            {
                cameraNormal.gameObject.SetActive(true);
                cameraFight.gameObject.SetActive(false);
                cenemytarget.gameObject.SetActive(false);
                //cameraNormal.Priority = 3;
                //cameraFight.Priority = 2;
                //cenemytarget.Priority = 1;
            }
        }
    }

}
