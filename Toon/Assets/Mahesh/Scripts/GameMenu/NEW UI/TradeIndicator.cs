using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeIndicator : MonoBehaviour
{
    public static TradeIndicator instance;

    public int qty;
    public Text qtyText;
    public Slider slider;
    public SliderState sliderState;

    public enum SliderState {
        InventoryToInventoryBox, InventoryBoxToInventory
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        slider = transform.GetChild(0).GetComponent<Slider>();
        qtyText = slider.transform.Find("QtyText").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.gameObject.SetActive(false);
    }
}
