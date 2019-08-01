using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {
    public static CameraMovement instance;

    /// <summary>
    /// target group untuk locking, dipake sama lookat cenemytarget
    /// </summary>
    public CinemachineTargetGroup ctg;

    //normal camera tanpa locking
    public CinemachineFreeLook cameraNormal;
    //camera ketika lagi fight
    public CinemachineFreeLook cameraFight;
    //kalo lg targeting bareng target group
    public CinemachineVirtualCamera cenemytarget;

    /// <summary>
    /// camera ketika lagi ngadep kebelakang ("dpad down")
    /// dipanggil di function character input pada ienumerator turning back
    /// </summary>
    public CinemachineFreeLook cameraTurnBack;

    /// <summary>
    /// ketika lock, dipanggil characterinput supaya player ngadep ke monstertarget
    /// characterinput tidak bisa rotate ketika sedang locking
    /// </summary>
    public bool isLocking;

    //dipake untuk characterinput supaya player tetep liat ke arah monster
    public Transform monsterTarget;
    //indicator ketika locking
    public GameObject targetIndicator; 

    [SerializeField]
    //mencari monster terdekat disekitar
    GameObject[] nearestMonster;
    //monster number untuk select monster dari nearestmonster
    int monsterNum;
    //ketika sedang cari monster
    bool getMonster;
    //ketika selesai locking
    bool endLock; 

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        cameraNormal.gameObject.SetActive(true);
        cameraFight.gameObject.SetActive(false);
        cenemytarget.gameObject.SetActive(false);
        cameraTurnBack.gameObject.SetActive(false);

        //set cameratarget ke grouptarget
        cenemytarget.LookAt = ctg.transform;
    }

    private void Update()
    {
        //locking
        if (Input.GetAxisRaw("LT Button") > 0)
        {
            CameraLock();
        }
        else if(endLock)
        {
            //endLock jadi false supaya tidak nge run function ini terus
            endLock = false;
            //matiin indicatornya
            targetIndicator.SetActive(false);
            //get monster jadiin false supaya bisa start locking lagi
            getMonster = false;
            //kondisi locking=false
            isLocking = false;
            ResetCamera();
        }
    }

    void CameraLock()
    {
        if (getMonster == false) //function pertama yang dipanggil sebelum locking
        {
            //getmonster jadi true supaya tidak nge run function ini terus
            getMonster = true;
            //endlock true untuk reset nanti setelah lepas input lt button
            endLock = true; 
            GetMonster();
        }
        //jika ada monster yang ditemukan
        if (nearestMonster.Length != 0) 
        {
            //tanda jika sedang targeting
            isLocking = true; 

            //untuk ganti target
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                monsterNum++;
                if (monsterNum > nearestMonster.Length - 1)
                {
                    monsterNum = 0;
                }
            }
            //set value monster target
            monsterTarget = nearestMonster[monsterNum].transform;
            //masukkin monster ke target kedua target group
            ctg.m_Targets[1].target = nearestMonster[monsterNum].transform; 

            //target indicator settings
            Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(nearestMonster[monsterNum].transform.position);
            targetIndicator.transform.position = targetScreenPoint;
            targetIndicator.SetActive(true);
        }
        else {
            //jika tidak ada monster yang ditemukan
            getMonster = false;
            isLocking = false;
            ResetCamera();
        }
    }

    /// <summary>
    /// function untuk mengubah camera
    /// </summary>
    public void ResetCamera()
    {
        if (!isLocking)
        {
            if (CharacterInput.instance.combatMode)
            {
                cameraNormal.gameObject.SetActive(false);
                cameraFight.gameObject.SetActive(true);
                cenemytarget.gameObject.SetActive(false);
            }
            else
            {
                cameraNormal.gameObject.SetActive(true);
                cameraFight.gameObject.SetActive(false);
                cenemytarget.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Untuk memulai mencari monster
    /// function ini dipanggil juga saat ada monster yang mati
    /// </summary>
    public void GetMonster()
    {
        if (isLocking) 
        {
            monsterTarget = null; 
            monsterNum = 0; 
            nearestMonster = GameObject.FindGameObjectsWithTag("Monster");

            //set camera menjadi targetmode
            cameraNormal.gameObject.SetActive(false);
            cameraFight.gameObject.SetActive(false);
            cenemytarget.gameObject.SetActive(true);
        }
    }
}
