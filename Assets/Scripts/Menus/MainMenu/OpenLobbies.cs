using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLobbies : MonoBehaviour
{
    public GameObject create, find, createMenu, findMenu;

    public void OpenLobbyList()
    {
        create.SetActive(true);
        find.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenCreateMenu()
    {
        createMenu.SetActive(true);
        findMenu.SetActive(false);
    }
    
    public void OpenFindMenu()
    {
        findMenu.SetActive(true);
        createMenu.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
