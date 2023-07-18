using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;


public class JoinLobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;

    public void JoinOnClick()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        SceneManager.LoadScene("Lobby");
    }
}
