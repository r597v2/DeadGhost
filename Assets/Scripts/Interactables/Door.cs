using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public AudioSource sound;

    private bool open;
    public float speed = 1f;
    public float rotationAmount = 90f;
    public float forwardDirection = -1.1f;

    private Vector3 startRotation;
    private Vector3 forward;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;
    }

    public void Interact()
    {
        if (!open) Open(Vector3.zero);
        else Close();
    }

    public void Open(Vector3 userPos)
    {
        if (!open)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);

            float dot = Vector3.Dot(forward, (userPos - transform.position).normalized);
            animationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot;

        if (forwardAmount >= forwardDirection)
        {
            endRot = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        else
        {
            endRot = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }

        open = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (open)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRotation);

        open = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    // public void ToggleDoor()
    // {
    //     open = !open;

    //     float time = 0f;
    //     Quaternion startRotation = transform.rotation;
    //     Quaternion endRotation;
    //     if (open) endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + 90f, 0));
    //     else endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - 90f, 0));

    //     while (time < 1)
    //     {
    //         transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
    //         time += Time.deltaTime * speed;
    //     }
    //     sound.pitch = Random.Range(0.5f, 1.5f);
    //     sound.Play();
    // }
}