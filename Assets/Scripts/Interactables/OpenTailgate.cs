using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTailgate : Interactable
{
    public Animator animator;
    public AudioSource doorSound;
    private bool doorOpen;

    protected override void Interact(int photonViewID)
    {
        if (!doorOpen)
        {
            animator.SetTrigger("Open");
            doorSound.Play();
            doorOpen = true;
        }
        else
        {
            animator.SetTrigger("Close");
            doorSound.Play();
            doorOpen = false;
        }
    }
}
