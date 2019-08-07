using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class yang menyimpan data untuk trade slider
/// karena trade slider tidak menyimpan data sama sekali
/// </summary>
public class TradeIndicator : MonoBehaviour
{
    public static TradeIndicator instance;
    
    //quantity yang disimpen oleh slider
    public int qty;
    //text yang diubah oleh slider
    public Text qtyText;
    //slider yang akan dipake oleh inventory dan inventory box
    public Slider slider;
    //slider state yang berpengaruh pada tradeslider
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
    
    void Start()
    {
        slider.gameObject.SetActive(false);
    }
}
