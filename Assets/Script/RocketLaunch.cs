using UnityEngine;
using TMPro;

public class RocketLaunch : MonoBehaviour
{
    public float throwForce = 20f; // Speed of the rocket
    public GameObject rocketPrefab; // Rocket prefab
    public TextMeshProUGUI statusText; // Reference to the TextMeshProUGUI UI element
    private Camera arCamera;
    private bool canThrow = false; // Tracks if throwing is enabled
    private float throwCooldown = 5f; // Time in seconds to wait before throwing again
    private float cooldownTimer = 0f; // Tracks the remaining cooldown time

    void Start()
    {
        arCamera = Camera.main; // Get the main camera
        cooldownTimer = throwCooldown; // Set the initial cooldown time
        UpdateStatusText(); // Update the status text at the start
    }

    void Update()
    {
        // If we are not able to throw, update the timer
        if (!canThrow)
        {
            cooldownTimer -= Time.deltaTime; // Decrement the cooldown timer
            if (cooldownTimer <= 0)
            {
                canThrow = true; // Enable throwing again after cooldown
                cooldownTimer = 0; // Reset cooldown timer
            }
        }

        // Update the UI status text with the current cooldown time
        UpdateStatusText();

        // If throwing is enabled and the screen is touched
        if (canThrow && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            Vector3 targetPosition = GetTouchWorldPosition();
            ThrowRocket(targetPosition);

            // Disable throwing and start the cooldown timer
            canThrow = false;
            cooldownTimer = throwCooldown; // Start cooldown
        }
    }

    Vector3 GetTouchWorldPosition()
    {
        // Get touch or mouse position
        Vector3 touchPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : (Vector3)Input.mousePosition;

        // Convert screen position to world position
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If it hits something, return the hit point
            return hit.point;
        }
        return ray.GetPoint(1000); // Default to a far distance if no hit (i.e., off the plane)
    }

    void ThrowRocket(Vector3 targetPosition)
    {
        // Instantiate the rocket at the camera's position
        GameObject rocket = Instantiate(rocketPrefab, arCamera.transform.position, Quaternion.identity);
        Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();

        // Calculate direction from the camera to the touch position
        Vector3 direction = (targetPosition - arCamera.transform.position).normalized;

        // Apply force to the rocket in that direction
        rocketRb.AddForce(direction * throwForce, ForceMode.VelocityChange);
    }

    void UpdateStatusText()
    {
        // Update the status text based on whether the rocket can be thrown
        if (canThrow)
        {
            statusText.text = "Rocket Ready! Tap to throw.";
        }
        else
        {
            statusText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer) + "s";
        }
    }
}
