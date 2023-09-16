using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNotepad : MonoBehaviour
{
    public GameObject notepad;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) notepad.SetActive(!notepad.activeInHierarchy);
    }
}
