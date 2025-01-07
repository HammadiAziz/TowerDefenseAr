using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMainBase : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed of the up-and-down movement
    public float floatAmplitude = 0.5f; // Range of the up-and-down movement
    public float rotationSpeed = 30f; // Speed of the rotation (degrees per second)

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Record the starting position
    }

    private void Update()
    {
        // Up-and-down movement
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotation
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
