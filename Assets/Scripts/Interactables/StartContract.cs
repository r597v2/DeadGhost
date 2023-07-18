using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartContract : Interactable
{
    private AudioSource startCarSound;

    protected override void Interact(int photonViewID)
    {
        startCarSound.Play();
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
}
