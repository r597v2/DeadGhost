using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    [Header("Bobbing Parameters")]
    [SerializeField] private Transform viewModel;           // Reference to the view model to apply bobbing.
    [SerializeField] private float bobFrequency = 2.0f;     // Frequency of the bobbing motion.
    [SerializeField] private float bobIntensity = 0.05f;    // Intensity of the bobbing motion.
    [SerializeField] private float sprintBobFrequency = 3.0f;// Frequency of the bobbing motion while sprinting.
    [SerializeField] private float sprintBobIntensity = 0.1f;// Intensity of the bobbing motion while sprinting.
    [SerializeField] private float idleBobFrequency = 0.5f; // Frequency of the idle breathing effect.
    [SerializeField] private float idleBobIntensity = 0.01f;// Intensity of the idle breathing effect.

    private Vector3 originalViewModelPosition;             // Original view model position (no bobbing).
    private Vector3 targetBobPosition;                     // Target view model position with bobbing.

    private void Start()
    {
        originalViewModelPosition = viewModel.localPosition;
        targetBobPosition = originalViewModelPosition;
    }

    private void Update()
    {
        // Calculate bobbing values based on player movement (can be replaced with your movement input logic).
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Determine the bobbing frequency and intensity based on the player's movement.
        float bobFactor = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
        bobFactor = Mathf.Clamp01(bobFactor);

        // Determine the current idle breathing intensity (less intense than movement bobbing).
        float idleFactor = Mathf.Abs(Mathf.Sin(Time.time * idleBobFrequency));

        // Combine the movement bobbing and idle breathing effects.
        float totalBobIntensity = bobIntensity * bobFactor + idleBobIntensity * idleFactor;

        // Calculate the horizontal and vertical bobbing offsets using sine functions.
        float horizontalOffset = Mathf.Sin(Time.time * (Input.GetKey(KeyCode.LeftShift) ? sprintBobFrequency : bobFrequency)) * totalBobIntensity;
        float verticalOffset = Mathf.Cos(Time.time * (Input.GetKey(KeyCode.LeftShift) ? sprintBobFrequency : bobFrequency) * 2) * totalBobIntensity;

        // Adjust the bobbing intensity when sprinting.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontalOffset *= sprintBobIntensity;
            verticalOffset *= sprintBobIntensity;
        }

        // Set the target bobbing position.
        targetBobPosition = originalViewModelPosition + new Vector3(horizontalOffset, verticalOffset, 0f);
    }

    private void LateUpdate()
    {
        // Smoothly move the view model to the target bobbing position.
        viewModel.localPosition = Vector3.Lerp(viewModel.localPosition, targetBobPosition, Time.deltaTime * 10f);
    }
}
