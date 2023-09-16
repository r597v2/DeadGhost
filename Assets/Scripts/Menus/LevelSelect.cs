using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour, IInteractable
{
    public GameObject levelSelectCanvas;
    public string selectedLevel;
    public GameObject destMarker;

    public void Interact()
    {
        levelSelectCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseLevelSelect()
    {
        levelSelectCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SelectLevel(string levelName)
    {
        selectedLevel = levelName;
    }
}