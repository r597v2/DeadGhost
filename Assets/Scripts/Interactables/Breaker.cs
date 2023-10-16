using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour, IInteractable
{
    private Lightswitch[] lightSwitches = new Lightswitch[]{};
    public bool breakerOn;
    private Animator animator;
    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        lightSwitches = FindObjectsOfType<Lightswitch>();
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        breakerOn = !breakerOn;
        animator.SetTrigger("Toggle");
        sound.Play();
    }
}