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
    // Start is called before the first frame update

    void Start()
    {
        select = KeyCode.Joystick1Button1;
        back = KeyCode.Joystick1Button0;
        deleteSave = KeyCode.Joystick1Button2;

        interact = KeyCode.Joystick1Button1;
        continueTalk = KeyCode.Joystick1Button0;
        openGameMenu = KeyCode.Joystick1Button7;
        jump = KeyCode.Joystick1Button0;
        putInventory = KeyCode.Joystick1Button2;
    }
}
