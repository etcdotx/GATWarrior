using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetup : MonoBehaviour
{
    public KeyCode select;
    public KeyCode back;
    public KeyCode deleteSave;
    public KeyCode interact;
    public KeyCode continueTalk;
    public KeyCode openGameMenu;
    public KeyCode jump;
    public KeyCode putInventory;
    public KeyCode attack;
    public KeyCode useItem;
    public KeyCode lockEnemy;
    // Start is called before the first frame update

    void Start()
    {
        back = KeyCode.Joystick1Button0;
        continueTalk = KeyCode.Joystick1Button0;
        jump = KeyCode.Joystick1Button0;

        interact = KeyCode.Joystick1Button1;
        select = KeyCode.Joystick1Button1;

        deleteSave = KeyCode.Joystick1Button2;
        putInventory = KeyCode.Joystick1Button2;
        attack = KeyCode.Joystick1Button2;

        useItem = KeyCode.Joystick1Button5;

        openGameMenu = KeyCode.Joystick1Button7;

        lockEnemy = KeyCode.Joystick1Button9;
    }
}
