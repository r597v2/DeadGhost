using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckTime : MonoBehaviourPunCallbacks
{
    public Animator animator;
    private bool checkingTime;
    public GameObject clock;

    public Transform cameraTransform;
    public Transform clockPivot;

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            if (!checkingTime)
            {
                clock.SetActive(true);
                Invoke("Check", 0.25f);
            }
            else
            {
                animator.SetTrigger("StopCheckingTime");
                checkingTime = false;
                Invoke("SetFalse", 0.5f);
            }
        }
    }

    private void SetFalse()
    {
        clock.SetActive(false);
    }
    private void Check()
    {
        animator.SetTrigger("CheckTime");
        checkingTime = true;
    }
}
