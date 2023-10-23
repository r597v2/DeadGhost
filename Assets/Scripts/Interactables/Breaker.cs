using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour, IInteractable
{
    private Lightswitch[] lightSwitches = new Lightswitch[] { };
    public bool breakerOn;
    private Animator animator;
    private AudioSource sound;
    public int lightsOn = 0;

    // Start is called before the first frame update
    void Start()
    {
        lightSwitches = FindObjectsOfType<Lightswitch>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (lightsOn > Decimal.ToInt32(lightSwitches.Length / 2))
        {
            breakerOn = false;
            // animator.SetTrigger("Toggle");
            foreach (Lightswitch lightswitch in lightSwitches)
            {
                if (lightswitch.switchOn) lightswitch.ToggleSwitch();
            }
        }
    }

    public void Interact()
    {
        breakerOn = !breakerOn;
        // animator.SetTrigger("Toggle");
        // sound.Play();
        if (!breakerOn)
        {
            foreach (Lightswitch lightSwitch in lightSwitches)
            {
                foreach (GameObject light in lightSwitch.lights)
                {
                    light.GetComponentInChildren<Light>(true).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (Lightswitch lightSwitch in lightSwitches)
            {
                if (lightSwitch.switchOn)
                {
                    foreach (GameObject light in lightSwitch.lights)
                    {
                        light.GetComponentInChildren<Light>(true).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}