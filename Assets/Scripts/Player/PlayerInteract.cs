using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviourPunCallbacks
{
    [SerializeField] private float distance = 2f;
    public GameObject crosshair;
    public Camera cam;

    public bool promptMessageSettingOn;
    // private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        // cam = GetComponent<MouseLook>().cam;
        // playerUI = GetComponent<PlayerUI>();
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
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (promptMessageSettingOn)
                {
                    // playerUI.UpdateText(interactable.promptMessage);
                }
                crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(7.5f, 7.5f);
                crosshair.GetComponent<Image>().color = new Color(245f, 245f, 245f, 255f);

                if (
                    (Input.GetMouseButtonDown(0) && interactable.interactType == Interactable.INTERACT_TYPE.INTERACT) // Interact type
                    || (Input.GetKeyDown(KeyCode.E) && interactable.interactType == Interactable.INTERACT_TYPE.PICKUP) // pickup type
                    )
                {
                    interactable.BaseIntereract(photonView.ViewID);
                }
            }
            else
            {
                crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
                crosshair.GetComponent<Image>().color = new Color(245f, 245f, 245f, 100f);
            }
        }
        else
        {
            crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
            crosshair.GetComponent<Image>().color = new Color(245f, 245f, 245f, 100f);
        }
    }
}