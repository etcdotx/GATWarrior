using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetup : MonoBehaviour
{
    [Header("A")]
    public KeyCode back;
    public KeyCode jump;

    [Header("B")]
    public KeyCode continueTalk;
    public KeyCode interact;
    public KeyCode select;
    public KeyCode buy;

    [Header("X")]
    public KeyCode deleteSave;
    public KeyCode putInventory;
    public KeyCode useItem;
    public KeyCode sendToBox;

    [Header("Y")]
    public KeyCode attack;

    [Header("RB")]
    public KeyCode shielding;

    [Header("RT")]

    [Header("LB")]

    [Header("LT")]
    public KeyCode lockEnemy;

    [Header("START")]
    public KeyCode openGameMenu;
    // Start is called before the first frame update

    void Start()
    {
        //A
        back = KeyCode.Joystick1Button0;
        jump = KeyCode.Joystick1Button0;

        //B
        continueTalk = KeyCode.Joystick1Button1;
        interact = KeyCode.Joystick1Button1;
        select = KeyCode.Joystick1Button1;
        buy = KeyCode.Joystick1Button1;

        //X
        deleteSave = KeyCode.Joystick1Button2;
        putInventory = KeyCode.Joystick1Button2;
        useItem = KeyCode.Joystick1Button2;
        sendToBox = KeyCode.Joystick1Button2;

        //Y
        attack = KeyCode.Joystick1Button3;

        //rb
        shielding = KeyCode.Joystick1Button5;

        //lt
        lockEnemy = KeyCode.Joystick1Button9;

        //start
        openGameMenu = KeyCode.Joystick1Button7;
    }
}
