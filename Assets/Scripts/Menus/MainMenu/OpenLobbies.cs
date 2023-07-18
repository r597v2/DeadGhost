using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLobbies : MonoBehaviour
{
    public GameObject createRoom;
    public GameObject roomListings;
    public GameObject quitGame;

    public void OpenLobbyList()
    {
        createRoom.SetActive(true);
        roomListings.SetActive(true);
        gameObject.SetActive(false);
        quitGame.SetActive(false);
    }
}
