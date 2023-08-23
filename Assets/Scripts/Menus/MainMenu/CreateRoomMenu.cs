using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _roomName;
    private int lobbyNumber = 0;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Update()
    {
        // Debug.Log(PhotonNetwork.CountOfRooms + " rooms currently active.");
        // if (PhotonNetwork.CurrentRoom != null)
        // {
        // Debug.Log("Currently in room: " + PhotonNetwork.CurrentRoom.Name);
        // }
        // else
        // {
        //     Debug.Log("Currently not in a room");
        // }
    }

    public void OnClick_CreateRoom()
    {
        // if(!PhotonNetwork.IsConnected)
        // {
        //     Debug.Log("Photon Network is not connected, connecting...");
        //     return;
        // }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = true;
        options.PlayerTtl = 3000;
        options.EmptyRoomTtl = 5000;
        _roomName.text = "test" + lobbyNumber;
        PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
        SceneManager.LoadScene("Lobby");
        lobbyNumber += 1;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed:" + message);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Joining lobby.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully Joined a Room.");
        Debug.Log("Room name:" + PhotonNetwork.CurrentRoom.Name + ". Players in this room: "
        + PhotonNetwork.CurrentRoom.PlayerCount + ". Players in all rooms: " + PhotonNetwork.PlayerList.Length);
    }
}
