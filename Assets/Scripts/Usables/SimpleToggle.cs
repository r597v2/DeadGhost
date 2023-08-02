using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleToggle : MonoBehaviour, IUsable
{
    public GameObject effects;
    public bool itemOn;
    public AudioSource sound;
    public bool hasOffSound;

    public void UseItem()
    {
        if (!itemOn)
        {
            effects.SetActive(true);
            itemOn = true;
            sound.pitch = Random.Range(0.75f, 1.5f);
            sound.Play();
        }
        else
        {
            effects.SetActive(false);
            itemOn = false;
            if (hasOffSound)
            {
                sound.pitch = Random.Range(0.75f, 1.5f);
                sound.Play();
            }
        }
    }
}
