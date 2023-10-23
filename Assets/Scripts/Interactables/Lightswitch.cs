using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : MonoBehaviour, IInteractable
{
    public GameObject[] lights = new GameObject[] { };

    private Animator animator;
    private AudioSource sound;
    public bool switchOn;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sound = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        ToggleSwitch();
        if (FindObjectOfType<Breaker>().breakerOn)
        {
            foreach (GameObject light in lights)
            {
                light.GetComponentInChildren<Light>(true).gameObject.SetActive(switchOn);
            }
        }
    }

    public void ToggleSwitch()
    {
        if (!switchOn)
        {
            animator.SetTrigger("On");
            switchOn = true;
            FindObjectOfType<Breaker>().lightsOn++;
        }
        else
        {
            animator.SetTrigger("Off");
            switchOn = false;
            FindObjectOfType<Breaker>().lightsOn--;
        }
        sound.Play();
    }
}