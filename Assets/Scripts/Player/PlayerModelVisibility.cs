using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerModelVisibility : MonoBehaviourPun
{
    public GameObject playerModel; // Reference to the player model GameObject
    public GameObject itemViewModels;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable the rendering of the player model for other players' cameras.
            Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            itemViewModels.SetActive(false);
        }
        else
        {
            // Enable the rendering of the player model for the local player's camera.
            Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            itemViewModels.SetActive(true);
        }
    }
}