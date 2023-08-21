using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartContract : MonoBehaviourPunCallbacks, IInteractable
{
    private AudioSource startCarSound;
    public LevelSelect levelSelect;

    public void Interact()
    {
        startCarSound.Play();
        if (levelSelect.selectedLevel != null)
        {
            Invoke("LoadSelectedContract", 2.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startCarSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadSelectedContract()
    {
        PhotonNetwork.LoadLevel(levelSelect.selectedLevel);
    }
}
