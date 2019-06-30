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
    public bool isSelected;

    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(InputSetup.instance.X))
            {
                Shop.instance.SendSelectedItem(this);
            }
        }
    }

    public void RefreshIndicator()
    {
        itemNameText.text = item.itemName;
        priceText.text = item.price.ToString() + " g";
        image.overrideSprite = item.itemImage;
    }
    public void OnCancel(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        UIManager.instance.StartCoroutine(UIManager.instance.ChangeState(UIManager.UIState.Gameplay));
        isSelected = false;
        Shop.instance.buttons.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedIndicator.SetActive(false);
        isSelected = false;
        Shop.instance.buttons.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
        Shop.instance.itemDescription.item = item;
        Shop.instance.itemDescription.RefreshDescription();
        selectedIndicator.SetActive(true);
        isSelected = true;
        Shop.instance.buttons.SetActive(true);

        int a = transform.GetSiblingIndex();
        int b = a / 7;
        Debug.Log(b);
        Shop.instance.shopContent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-b * 410, 0, 0);
    }

    public void BuyItem() {
        Shop.instance.BuySelectedItem(this);
    }


}
