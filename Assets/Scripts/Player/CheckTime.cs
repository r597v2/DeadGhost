using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckTime : MonoBehaviourPunCallbacks
{
    public Animator animator;
    private bool checkingTime;

    public Transform cameraTransform;
    public Transform clockPivot;

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            if (!checkingTime)
            {
                animator.SetTrigger("CheckTime");
                checkingTime = true;
            }
            else
            {
                animator.SetTrigger("StopCheckingTime");
                checkingTime = false;
            }
        }
    }
}
