using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClosePauseMenu();
        }
    }

    public void OpenClosePauseMenu()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            player.GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pauseMenu.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
