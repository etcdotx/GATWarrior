using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRayCast : MonoBehaviour
{
    public Player player;
    public GameObject mainCamera;
    public GameObject xButton;
    public Text interactText;

    public float maxRayDistance;
    public Vector3 raycastOffset;

    public InputSetup inputSetup;
    public ShowDialog showDialog;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inputSetup = GameObject.FindGameObjectWithTag("InputSetup").GetComponent<InputSetup>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        showDialog = GameObject.FindGameObjectWithTag("ShowDialog").GetComponent<ShowDialog>();

        //**xbutton dan interact text harus di enable pas gantiscene
        xButton = GameObject.Find("XButton");
        interactText = GameObject.Find("InteractText").GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if (GameStatus.IsPaused == false && GameStatus.isTalking == false)
        {
            Ray ray = new Ray(transform.position + raycastOffset, transform.forward + raycastOffset);
            RaycastHit hit;
            Debug.DrawLine(transform.position + raycastOffset, transform.position + raycastOffset + transform.forward * maxRayDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                try
                {
                    Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                    if (interactable.isInteractable == true)
                    {
                        interactText.text = interactable.interactText;
                        ShowButton();
                        if (Input.GetKeyDown(inputSetup.interact) && interactable.isTalking == true)
                        {
                            GameStatus.isTalking = true;
                            HideButton();
                            mainCamera.transform.position = interactable.interactView.transform.position;
                            mainCamera.transform.rotation = interactable.interactView.transform.rotation;
                            Debug.Log(hit.collider.gameObject.name + " is interactable");
                            showDialog.startDialog(interactable.dialog);
                        }
                        if (Input.GetKeyDown(inputSetup.interact) && interactable.isCollectable == true)
                        {
                            int itemID = interactable.gameObject.GetComponent<Interactable>().itemID;
                            if (ItemDataBase.item == null)
                                ItemDataBase.item = new List<Item>();

                            for (int i = 0; i < ItemDataBase.item.Count; i++)
                            {
                                if (ItemDataBase.item[i].id == itemID)
                                {
                                    player.AddItem(ItemDataBase.item[i]);
                                    break;
                                }
                            }
                            Destroy(interactable.gameObject);
                        }
                    }
                }
                catch
                {
                    Debug.Log(hit.collider.gameObject.name + " is not interactable");
                }
            }
            else
            {
               HideButton();//**bikin eror kalo ganti scene, player gabisa search button dan textnya
            }
        }

    }

    void ShowButton()
    {
        interactText.gameObject.SetActive(true);
        xButton.gameObject.SetActive(true);
    }

    void HideButton()
    {
        interactText.gameObject.SetActive(false);
        xButton.gameObject.SetActive(false);
    }
}
