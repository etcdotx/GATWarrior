using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IMonsterAttack
{
    bool isAttacking { get; set; }
    bool onCombat { get; set; }
    bool passive { get; set; }
    bool aggressive { get; set; }
    bool playerOnSight { get; set; }
    Transform target { get; set; }
    
}
