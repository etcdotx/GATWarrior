using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopIndicator : MonoBehaviour, ISelectHandler, ICancelHandler, IDeselectHandler
{
    public Item item;
    public Image image;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public GameObject selectedIndicator;

    public void RefreshIndicator()
    {
        itemNameText.text = item.itemName;
        priceText.text = item.price.ToString() + " g";
        image.overrideSprite = item.itemImage;
    }
    public void OnCancel(BaseEventData eventData)
    {
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedIndicator.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
        Shop.instance.itemDescription.item = item;
        Shop.instance.itemDescription.RefreshDescription();
        selectedIndicator.SetActive(true);
    }
}
