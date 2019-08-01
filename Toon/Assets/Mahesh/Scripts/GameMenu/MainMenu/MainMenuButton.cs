using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public ButtonName buttonName;
    public GameObject selectIndicator;
    public enum ButtonName {
        startgame, exitgame
    }

    // Start is called before the first frame update
    void Start()
    {
        selectIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectIndicator.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectIndicator.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (buttonName == ButtonName.startgame)
        {
            MainMenuScript.instance.MoveBackground(true);
        }
    }
}
