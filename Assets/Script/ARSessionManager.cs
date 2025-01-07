using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionManager : MonoBehaviour
{
    private ARSession arSession;
    private Camera arCamera;

    void Start()
    {
        // Find the AR session and camera
        arSession = FindObjectOfType<ARSession>();
        arCamera = Camera.main; // Find the main camera

        // Ensure the AR session is valid
        if (arSession == null)
        {
            Debug.LogError("ARSession not found in the scene.");
            return;
        }

        // Make sure AR session is reset and starts correctly
        if (arSession != null)
        {
            arSession.Reset(); // Reset AR session to ensure it's ready
        }

        // Ensure AR camera persists across scene reloads
        if (arCamera != null)
        {
            DontDestroyOnLoad(arCamera); // Prevent the camera from being destroyed
        }
    }

    void OnEnable()
    {
        // Check if AR session is already running, if not, start it
        if (arSession != null && arSession.subsystem != null && !arSession.subsystem.running)
        {
            arSession.Reset(); // Reset AR session if it's not running
        }
    }

    void Update()
    {
        // Check if the camera is missing or destroyed
        if (arCamera == null)
        {
            Debug.LogError("AR Camera has been destroyed or is missing.");
        }
        else
        {
            // Log the camera state to verify it is working
            Debug.Log("AR Camera is active and working.");
        }
    }
}
