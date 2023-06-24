using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    public float maxStamina = 30f;
    private float stamina;
    public float dValue = 10f;
    // [Space]
    // public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0f && rb.velocity != new Vector3(0f, rb.velocity.y, 0f))
        {
            speed = 5f;
            stamina -= dValue * Time.deltaTime;
        }
        else speed = 2.5f;

        if (stamina <= 0f) stamina = 0f;

        Vector3 moveVector = transform.TransformDirection(movementInput) * speed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

        // if (Input.GetKeyDown(KeyCode.Space) && Physics.CheckSphere(groundCheck.position, 0.1f, groundMask))
        // rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        // {
        //     animator.SetBool(speed == 5f ? "SprintF" : "WalkF", true);
        // }
        // if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        // {
        //     animator.SetBool("SprintF", false);
        //     animator.SetBool("WalkF", false);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     animator.SetBool(speed == 5f ? "SprintB" : "WalkB", true);
        // }
        // if (Input.GetKeyUp(KeyCode.S))
        // {
        //     animator.SetBool("SprintB", false);
        //     animator.SetBool("WalkB", false);
        // }

        // if (stamina <= 0f) animator.SetBool("SprintF", false);
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

