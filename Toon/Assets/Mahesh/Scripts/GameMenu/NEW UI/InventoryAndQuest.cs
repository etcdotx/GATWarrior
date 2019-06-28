using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryAndQuest : MonoBehaviour
{
    public EventSystem asd;
    public float itemTotal;
    public float itemNum;
    public Scrollbar scrollbar;
    public GameObject apa;
    public GameObject quest;
    public GameObject invenFirst;
    public GameObject questfirst;
    public Curstate curstate;

    public enum Curstate {
        onQuest, onInvent
    }


    void Start()
    {
        curstate = Curstate.onInvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (curstate == Curstate.onQuest)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                back();
                
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        //set the scrollbar position by selected item
        if (itemNum == 1)
            itemNum = 0;
        scrollbar.value = 1.0f - (itemNum / itemTotal);
        Debug.Log(this.gameObject.name + " was selected");

    }

    public void open1() {
        apa.SetActive(false);
        quest.SetActive(true);
        asd.SetSelectedGameObject(questfirst);
        curstate = Curstate.onQuest;
    }

    public void back()
    {
        apa.SetActive(true);
        quest.SetActive(false);
        asd.SetSelectedGameObject(invenFirst);
        curstate = Curstate.onInvent;
    }
}
