using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSway : MonoBehaviourPunCallbacks
{
    public float amount;
    public float maxAmount;
    public float smoothing;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        
        float mouseX = -Input.GetAxis("Mouse X") * amount;
        float mouseY = -Input.GetAxis("Mouse Y") * amount;

        mouseX = Mathf.Clamp(mouseX, -maxAmount, maxAmount);
        mouseY = Mathf.Clamp(mouseY, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothing);
    }
}
