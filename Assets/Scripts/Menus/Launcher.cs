using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;
    SpawnSpot[] spawnSpots;

    // public Ghost ghost;

    private int numOfItemSpawnSpots;

    // Start is called before the first frame update
    void Start()
    {
        spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnPlayer();

            if (SceneManager.GetActiveScene().name != "Lobby")
            {
                // ghost.PickGhostType();
            }
        }
        else
        // {
        //     PhotonNetwork.ConnectUsingSettings();
        // }

            PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        // PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created a new room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Warning: failed to join room. " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined: " + newPlayer.ToStringFull());
        Debug.Log("Players in this room: " + PhotonNetwork.CurrentRoom.PlayerCount + ". Players in all rooms: " + PhotonNetwork.PlayerList.Length);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player left");
        Debug.Log("Players in this room: " + PhotonNetwork.CurrentRoom.PlayerCount + ". Players in all rooms: " + PhotonNetwork.PlayerList.Length);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully Joined a Room.");
        Debug.Log("Players in this room: " + PhotonNetwork.CurrentRoom.PlayerCount + ". Players in all rooms: " + PhotonNetwork.PlayerList.Length);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        SpawnSpot mySpawnSpot = spawnSpots[Random.Range(0, spawnSpots.Length)];
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);

        player.transform.Find("Camera").gameObject.SetActive(true);
        player.transform.Find("Canvas").gameObject.SetActive(true);
    }
}
