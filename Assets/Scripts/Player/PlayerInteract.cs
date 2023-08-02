using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviourPunCallbacks
{
    [SerializeField] private float distance = 2f;
    public GameObject crosshair;
    public GameObject ePrompt;
    public Camera cam;
    private PlayerInventory playerInventory;

    public bool promptMessageSettingOn;
    // private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        // cam = GetComponent<MouseLook>().cam;
        // playerUI = GetComponent<PlayerUI>();
        playerInventory = GetComponent<PlayerInventory>();
        promptMessageSettingOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // playerUI.UpdateText(string.Empty);
        //create a ray at the center of the camera, shooting outwards.
        Ray interactRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(interactRay.origin, interactRay.direction * distance, Color.green);
        RaycastHit hitInfo; //variable to store collision info
        if (Physics.Raycast(interactRay, out hitInfo, distance))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null || hitInfo.collider.gameObject.CompareTag("Pickable"))
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (promptMessageSettingOn)
                {
                    // playerUI.UpdateText(interactable.promptMessage);
                    ePrompt.SetActive(true);
                }
                else
                {
                    crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(7.5f, 7.5f);
                    crosshair.GetComponent<Image>().color = new Color(crosshair.GetComponent<Image>().color.r, crosshair.GetComponent<Image>().color.g,
                    crosshair.GetComponent<Image>().color.b, 50f);
                }

                if (Input.GetKeyDown(KeyCode.E)) // interact and pickup
                {
                    if (interactable != null && interactable.interactType == Interactable.INTERACT_TYPE.INTERACT)
                    {
                        interactable.BaseIntereract(photonView.ViewID);
                    }
                    else if (hitInfo.collider.CompareTag("Pickable"))
                    {
                        PickUpItem(hitInfo.collider.GetComponent<Item>(), hitInfo);
                    }
                }
            }
            else
            {
                crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
                crosshair.GetComponent<Image>().color = new Color(crosshair.GetComponent<Image>().color.r, crosshair.GetComponent<Image>().color.g,
                    crosshair.GetComponent<Image>().color.b, 255f);
                ePrompt.SetActive(false);
            }
        }
        else
        {
            crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
            crosshair.GetComponent<Image>().color = new Color(crosshair.GetComponent<Image>().color.r, crosshair.GetComponent<Image>().color.g,
                    crosshair.GetComponent<Image>().color.b, 255f);
            ePrompt.SetActive(false);
        }
    }

    private void PickUpItem(Item item, RaycastHit hitInfo)
    {
        if (GetNextAvailableSlot() != null)
        {
            playerInventory.inventoryList[(int)GetNextAvailableSlot()] = item.itemScriptableObject.itemType;
            playerInventory.NewItemSelected();
            Destroy(hitInfo.collider.gameObject);
        }
    }

    private int? GetNextAvailableSlot()
    {
        for (int i = playerInventory.selectedItem; i <= 3; i++)
        {
            if (playerInventory.inventoryList[i] == ItemType.Empty)
            {
                return i;
            }
            else if (i == 3 && playerInventory.inventoryList[i] != ItemType.Empty)
            {
                i = -1;
            }
        }
        return null;
    }

    public void TogglePromptMesssages(bool setting)
    {
        if (setting)
        {
            promptMessageSettingOn = true;
        }
        else promptMessageSettingOn = false;
    }
}