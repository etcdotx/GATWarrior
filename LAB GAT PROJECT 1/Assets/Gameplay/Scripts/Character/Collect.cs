using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void CollectObject(Interactable interactable)
    {
        //mengecek id dari item tersebut
        int itemID = interactable.itemID[0];
        interactable.itemID.RemoveAt(0);

        for (int i = 0; i < ItemDataBase.item.Count; i++)
        {
            //jika item yang ada didatabase sesuai dengan item yang diinteract
            //maka item tersebut dimasukkan kedalam koleksi item player
            if (ItemDataBase.item[i].id == itemID)
            {
                playerData.AddItem(ItemDataBase.item[i]);
                break;
            }
        }
        //gameobject item yang ada di hierarchy dihancurkan
        if(interactable.itemID.Count==0)
            Destroy(interactable.gameObject);
    }
}
