using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTailgate : MonoBehaviour, IInteractable
{
    public Animator animator;
    public AudioSource doorSound;
    private bool doorOpen;

    public void Interact()
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
