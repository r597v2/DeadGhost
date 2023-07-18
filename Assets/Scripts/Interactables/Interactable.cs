using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPun
{
    //message displayed to the player when looking at an interactable
    public string promptMessage;
    // Name used by RPC
    public string idName = "DEFAULT";

    public enum INTERACT_TYPE
    {
        INTERACT,
        PICKUP
    }

    [SerializeField]
    public INTERACT_TYPE interactType = INTERACT_TYPE.INTERACT;

    public void BaseIntereract(int photonViewID)
    {
        if (photonView != null)
        {
            photonView.RPC("RPC_Interact", RpcTarget.AllBuffered, idName, photonViewID);
        }
        else
        {
            Interact(photonViewID);
        }
    }

    [PunRPC]
    protected void RPC_Interact(string idName, int photonViewID)
    {
        Interactable target = null;
        Interactable[] allInteractables = FindObjectsOfType<Interactable>();
        foreach (Interactable interactable in allInteractables)
        {
            if (interactable.idName == idName)
            {
                target = interactable;
                break;
            }
        }

        target?.Interact(photonViewID);
    }

    protected GameObject FindPlayerGameObjectByPhotonViewID(int photonViewID)
    {
        GameObject playerGameObject = null;
        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject gameObj in playerGameObjects)
        {
            if (gameObj.GetPhotonView().ViewID == photonViewID)
            {
                playerGameObject = gameObj;
                break;
            }
        }

        return playerGameObject;
    }

    protected virtual void Interact(int photonViewID)
    {
        //template function to be overridden by subclasses
    }
}
