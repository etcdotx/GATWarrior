using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing : MonoBehaviour
{
    MonsterStatus monsterStatus;
    NewMonsterMovement monsterMovement;
    MonsterAttack monsterAttack;

    /// <summary>
    /// 1. green slime
    /// 2. red slime
    /// </summary>
    public Transform[] spawnPrefab;
    public float spawnTime;
    float curTime;

    private void Awake()
    {
        monsterStatus = GetComponent<MonsterStatus>();
        monsterMovement = GetComponent<NewMonsterMovement>();
        monsterAttack = GetComponent<MonsterAttack>();
    }

    void Start()
    {
        curTime = spawnTime;
    }
    void Update()
    {
        if (monsterAttack.onCombat)
        {
            if (curTime <= 0)
            {
                if (monsterStatus.hp > 0)
                {
                    if (monsterStatus.hp > monsterStatus.maxHp / 2)
                    {
                        Spawn1();
                    }
                    else if (monsterStatus.hp > monsterStatus.maxHp / 4)
                    {
                        Spawn2();
                    }
                    else
                    {
                        Spawn3();
                    }
                }
                curTime = spawnTime;
            }

            curTime -= Time.fixedDeltaTime;
        }
    }

    void Spawn1() {
        for (int i = 0; i < 1; i++)
        {
            Instantiate(spawnPrefab[1], transform.position, Quaternion.identity);
        }
    }
    void Spawn2()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(spawnPrefab[1], transform.position, Quaternion.identity);
        }
    }
    void Spawn3()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(spawnPrefab[1], transform.position, Quaternion.identity);
        }
    }
}
