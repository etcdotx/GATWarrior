using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeSlider : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        gameObject.SetActive(false);
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UISelectClip);
        if (TradeIndicator.instance.sliderState == TradeIndicator.SliderState.InventoryToInventoryBox)
        {
            InventoryBox.instance.PlaceItem(Inventory.instance.temporaryItem, TradeIndicator.instance.qty);
            UIManager.instance.eventSystem.SetSelectedGameObject(Inventory.instance.lastSelectedInventory);
        }
        else if (TradeIndicator.instance.sliderState == TradeIndicator.SliderState.InventoryBoxToInventory)
        {
            Inventory.instance.PlaceItem(InventoryBox.instance.temporaryItem, TradeIndicator.instance.qty);
            UIManager.instance.eventSystem.SetSelectedGameObject(InventoryBox.instance.lastSelectedInventory);
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        gameObject.SetActive(false);
        if (TradeIndicator.instance.sliderState == TradeIndicator.SliderState.InventoryToInventoryBox)
        {
            UIManager.instance.eventSystem.SetSelectedGameObject(Inventory.instance.lastSelectedInventory);
        }
        else if (TradeIndicator.instance.sliderState == TradeIndicator.SliderState.InventoryBoxToInventory)
        {
            UIManager.instance.eventSystem.SetSelectedGameObject(InventoryBox.instance.lastSelectedInventory);
        }
    }

    public void ChangeQty()
    {
        TradeIndicator.instance.qty = (int)TradeIndicator.instance.slider.value;
        TradeIndicator.instance.qtyText.text = TradeIndicator.instance.qty.ToString();
    }
}
