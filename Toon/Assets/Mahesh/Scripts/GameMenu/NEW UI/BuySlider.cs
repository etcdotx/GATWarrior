using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuySlider : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    public Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnCancel(BaseEventData eventData)
    {
        BuyConfirmation.instance.view.SetActive(false);
        UIManager.instance.eventSystem.SetSelectedGameObject(Shop.instance.lastSelected);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.CoinSound);
        if (BuyConfirmation.instance.confirmationState == BuyConfirmation.ConfirmationState.Buy)
            BuyConfirmation.instance.ConfirmBuy();
        else if (BuyConfirmation.instance.confirmationState == BuyConfirmation.ConfirmationState.SendToBox)
            BuyConfirmation.instance.ConfirmSend();
        else if (BuyConfirmation.instance.confirmationState == BuyConfirmation.ConfirmationState.Sell)
            BuyConfirmation.instance.ConfirmSell();

        UIManager.instance.eventSystem.SetSelectedGameObject(Shop.instance.lastSelected);
    }

    /// <summary>
    /// dipanggil dari button on value changed, yaitu ketika value pada slider berubah
    /// quantity -> quantity slider
    /// </summary>
    public void RefreshDetail()
    {
        SoundList.instance.UIAudioSource.PlayOneShot(SoundList.instance.UINavClip);
        BuyConfirmation.instance.setQty = (int)slider.value;
        BuyConfirmation.instance.RefreshText();
    }
}
