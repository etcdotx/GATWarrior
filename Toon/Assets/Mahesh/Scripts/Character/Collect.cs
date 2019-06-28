using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public void CollectObject(Interactable interactable)
    {
        //mengecek id dari item tersebut
        Item item = interactable.items[0];
        bool itemExist = false;

        for (int i = 0; i < PlayerData.instance.inventoryItem.Count; i++)
        {
            if (PlayerData.instance.inventoryItem[i].id == item.id)
                if (PlayerData.instance.inventoryItem[i].quantity == PlayerData.instance.inventoryItem[i].maxQuantityOnInventory)
                {
                    Debug.Log("you cannot carry more " + item.itemName);
                    return;
                }
                else {
                    for (int j = 0; j < ItemDataBase.item.Count; j++)
                    {
                        //jika item yang ada didatabase sesuai dengan item yang diinteract
                        //maka item tersebut dimasukkan kedalam koleksi item player
                        if (ItemDataBase.item[j].id == item.id)
                        {
                            interactable.items.RemoveAt(0);
                            PlayerData.instance.AddItem(ItemDataBase.item[j]);
                            itemExist = true;
                            break;
                        }
                    }
                    break;                    
                }
        }

        if (!itemExist)
        {
            bool thereIsSpace = false;
            for (int i = 0; i < Inventory.instance.inventoryIndicator.Length; i++)
            {
                if (Inventory.instance.inventoryIndicator[i].item == null)
                {
                    thereIsSpace = true;
                    break;
                }
            }
            if (!thereIsSpace)
            {
                Debug.Log("item space is full");
                return;
            }

            for (int j = 0; j < ItemDataBase.item.Count; j++)
            {
                //jika item yang ada didatabase sesuai dengan item yang diinteract
                //maka item tersebut dimasukkan kedalam koleksi item player
                if (ItemDataBase.item[j].id == item.id)
                {
                    interactable.items.RemoveAt(0);
                    PlayerData.instance.AddItem(ItemDataBase.item[j]);
                    break;
                }
            }
        }

        //gameobject item yang ada di hierarchy dihancurkan
        if (interactable.items.Count==0)
            Destroy(interactable.gameObject);
    }
}
