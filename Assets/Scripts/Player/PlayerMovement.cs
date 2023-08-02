using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    private Vector3 movementInput;
    private Vector2 mouseInput;
    private float xRot;
    private bool crouched;
    [Space]
    public GameObject cam;
    private Rigidbody rb;
    public float speed;
    public float sens;
    // public float jumpForce;
    // private LayerMask groundMask;
    // public Transform groundCheck;
    [Space]
    public float maxEnergy;
    private float energy;
    public float dValue;
    public Slider energyBar;
    public Image energyBarFill;
    public Image energyBarBackground;
    private bool breathingSoundPlaying;
    [Space]
    public SimpleToggle lighterToggle;
    public SimpleToggle oldLanternToggle;
    public SimpleToggle boxLanternToggle;
    [Space]
    public Image tempFill;
    private float maxTemp;
    private float temperature;
    [SerializeField] private float shiverSpeed;
    public float shiverIntensity;
    private Vector3 camOrigionalPos;
    public float camBobFreq;
    public float camBobAmplitude;
    [Space]
    public AudioSource footstepSound;
    public List<AudioClip> footstepSounds = new List<AudioClip>();
    [Space]
    public AudioSource breathingSound;
    public AudioSource coldSound;

    // [Space]
    // public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        energyBarFill.CrossFadeAlpha(0f, 0.1f, false);
        energyBarBackground.CrossFadeAlpha(0f, 0.1f, false);
        energyBar.value = maxEnergy;
        camOrigionalPos = cam.transform.localPosition;
        maxTemp = tempFill.fillAmount;
        temperature = maxTemp;
        energy = maxEnergy;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lobby") temperature = maxTemp;

        tempFill.fillAmount = temperature;
        temperature -= 0.0001f;

        energyBarFill.CrossFadeAlpha(0f, 0.25f, false);
        energyBarBackground.CrossFadeAlpha(0f, 0.25f, false);

        if (temperature <= 0)
        {
            energy = 0f;
            speed = 0.8f;
        }
        if (temperature <= 3.33f && temperature > 0) speed = 1.25f;

        if (lighterToggle.itemOn) temperature += 0.0005f;
        if (oldLanternToggle.itemOn) temperature += 0.001f;
        if (boxLanternToggle.itemOn) temperature += 0.0075f;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!crouched)
            {
                crouched = true;
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                GetComponent<CapsuleCollider>().height /= 2.5f;
                GetComponent<CapsuleCollider>().center *= 1.5f;
            }
            else
            {
                crouched = false;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.6610433f, transform.position.z);
                GetComponent<CapsuleCollider>().height *= 2.5f;
                GetComponent<CapsuleCollider>().center /= 1.5f;
            }
        }

        movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera();

        if (temperature <= 0.25f)
        {
            CameraShiver();
            coldSound.enabled = true;
        }
        else
        {
            cam.transform.localPosition = camOrigionalPos;
            coldSound.enabled = false;
        }

        if (temperature >= maxTemp) temperature = maxTemp;
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift) && energy > 0 && rb.velocity != new Vector3(0f, rb.velocity.y, 0f) && !crouched)
        {
            speed = 2.25f;
            energy -= dValue * Time.deltaTime;
            energyBar.value -= dValue * Time.deltaTime;
            temperature += 0.0005f;
        }
        else if (!crouched) speed = 1.5f;

        if (Input.GetKey(KeyCode.LeftShift) && energy > 0 && rb.velocity != new Vector3(0f, rb.velocity.y, 0f) && crouched)
        {
            speed = 1.75f;
            energy -= dValue * Time.deltaTime;
            energyBar.value -= dValue * Time.deltaTime;
            temperature += 0.0005f;
        }
        else if (crouched) speed = 1f;

        if (speed != 2.25f || speed != 1.75f)
        {
            energy += (0.25f * dValue * Time.deltaTime);
            energyBar.value += (0.25f * dValue * Time.deltaTime);
        }

        if (energy <= 0 || energyBar.value == 0)
        {
            energy = 0;
            if (!crouched) speed = 1.5f;
            else speed = 1f;


            if (!breathingSoundPlaying)
            {
                breathingSound.Play();
                breathingSoundPlaying = true;
                StartCoroutine(SetBoolFalse(breathingSoundPlaying, 3.25f));
            }
        }

        Vector3 moveVector = transform.TransformDirection(movementInput) * speed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);


        if (rb.velocity != new Vector3(0f, rb.velocity.y, 0f))
        {
            RaycastHit hit;
            Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, float.PositiveInfinity);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<FootstepSound>() != null)
            {
                if (footstepSound.clip != hit.collider.gameObject.GetComponent<FootstepSound>().sound)
                {
                    footstepSound.enabled = false;
                }
                footstepSound.clip = hit.collider.gameObject.GetComponent<FootstepSound>().sound;
            }

            footstepSound.enabled = true;
            footstepSound.pitch = Random.Range(0.75f, 1.25f);
        }
        else
        {
            footstepSound.enabled = false;
        }

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

    private void CameraShiver()
    {
        float shiverHorizontalOffset = Mathf.Sin(Time.time * shiverSpeed) * shiverIntensity;

        // Apply the shivering effect to the camera's local position
        cam.transform.localPosition = camOrigionalPos + new Vector3(shiverHorizontalOffset, 0f, 0f);
    }

    private IEnumerator SetBoolFalse(bool param, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        param = false;
    }

    public void OnEnergyBarValueChanged()
    {
        energyBarFill.CrossFadeAlpha(63.75f, 0.1f, false);
        energyBarBackground.CrossFadeAlpha(31.875f, 0.1f, false);
    }
}

