using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    private Vector3 movementInput;
    private Vector2 mouseInput;
    private float xRot;
    [Space]
    public GameObject cam;
    private Rigidbody rb;
    public float speed;
    public float sens;
    // public float jumpForce;
    // private LayerMask groundMask;
    // public Transform groundCheck;
    [Space]
    public float maxEnergy = 2.125f;
    private float energy;
    public float dValue = 10f;
    [Space]
    public int maxTemp = 10;
    private int temperature;
    [Space]
    public AudioSource footstepSound;
    // [Space]
    // public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        energy = maxEnergy;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift) && energy > 0 && rb.velocity != new Vector3(0f, rb.velocity.y, 0f))
        {
            speed = 2.25f;
            energy -= dValue * Time.deltaTime;
        }
        else speed = 1.5f;

        if (energy <= 0) energy = 0;

        Vector3 moveVector = transform.TransformDirection(movementInput) * speed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

        if (rb.velocity != new Vector3(0f, rb.velocity.y, 0f))
        {
            footstepSound.enabled = true;
            footstepSound.pitch = Random.Range(0.75f, 1.25f);
        }
        else footstepSound.enabled = false;

        // if (Input.GetKeyDown(KeyCode.Space) && Physics.CheckSphere(groundCheck.position, 0.1f, groundMask))
        // rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        // {
        //     animator.SetBool("WalkF", true);
        // }
        // if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        // {
        //     animator.SetBool("WalkF", false);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     animator.SetBool("WalkB", true);
        // }
        // if (Input.GetKeyUp(KeyCode.S))
        // {
        //     animator.SetBool("WalkB", false);
        // }
    }

    private void MoveCamera()
    {
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        xRot -= mouseInput.y * sens;

        transform.Rotate(0f, mouseInput.x * sens, 0f);
        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        // animator.SetFloat("HeadAngle", xRot);
    }
}

