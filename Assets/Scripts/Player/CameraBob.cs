using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 15.0f;     // Speed of the camera bobbing.
    [SerializeField] private float bobbingAmount = 0.001f;   // Amount of camera bobbing.

    private float timer = 0.0f;
    private float midpoint = 0.0f;

    private void Start()
    {
        midpoint = transform.localPosition.y;
    }

    private void Update()
    {
        // Calculate the bobbing motion
        if (Mathf.Approximately(Input.GetAxis("Horizontal"), 0f) && Mathf.Approximately(Input.GetAxis("Vertical"), 0f))
        {
            // Reset the timer and bring the camera back to the midpoint
            timer = 0.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, midpoint, transform.localPosition.z);
        }
        else
        {
            // Increment the timer based on a fixed value
            timer += bobbingSpeed * Time.deltaTime;

            // Calculate the vertical bobbing amount using a sine function
            float bobbingOffset = Mathf.Sin(timer) * bobbingAmount;

            // Apply the bobbing to the camera's local position
            transform.localPosition = new Vector3(transform.localPosition.x, midpoint + bobbingOffset, transform.localPosition.z);
        }
    }
}
