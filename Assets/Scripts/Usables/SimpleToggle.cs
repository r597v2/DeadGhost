using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleToggle : MonoBehaviour, IUsable
{
    [Header("Settings")]
    public GameObject[] effects = new GameObject[] { };
    public bool itemOn;
    public AudioSource sound;
    public bool hasOffSound;
    public ItemType itemRequirement;
    public bool hasItemRequirement;
    [Space]
    [Header("References")]
    public PlayerInventory playerInventory;

    public void UseItem()
    {
        if (!itemOn)
        {
            if (hasItemRequirement)
            {
                if (playerInventory.HasItem(itemRequirement))
                {
                    foreach (GameObject effect in effects)
                    {
                        effect.SetActive(true);
                    }

                    itemOn = true;
                    if (sound != null)
                    {
                        sound.pitch = Random.Range(0.75f, 1.5f);
                        sound.Play();
                    }
                }
            }
            else
            {
                foreach (GameObject effect in effects)
                {
                    effect.SetActive(true);
                }

                itemOn = true;
                if (sound != null)
                {
                    sound.pitch = Random.Range(0.75f, 1.5f);
                    sound.Play();
                }
            }
        }
        else
        {
            foreach (GameObject effect in effects)
            {
                effect.SetActive(false);
            }

            itemOn = false;
            if (hasOffSound && sound != null)
            {
                sound.pitch = Random.Range(0.75f, 1.5f);
                sound.Play();
            }
        }
    }
}
