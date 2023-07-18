using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeCharacter : MonoBehaviourPunCallbacks
{
    public GameObject[] characters = new GameObject[]{};
    public int characterNum;

    public void SelectCharacter(int character)
    {
        foreach (GameObject c in characters) c.SetActive(false);

        characters[character].SetActive(true);
    }
}
