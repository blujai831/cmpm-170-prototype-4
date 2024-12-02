using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerDrag : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    private bool isDragging = false;

    // Set this position for snapping (can be updated later)
    public Vector3 snapPosition = Vector3.zero;

    private float fixedZPosition; // Store the fixed Z position

    private void Start()
    {
        // Get the main camera and Rigidbody
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        // Disable gravity for the Rigidbody
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Prevent physics from interfering
        }

        // Store the initial Z position of the capsule
        fixedZPosition = transform.position.z;
    }

    private void Update()
    {
        // Check for mouse click and release
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            if (isDragging)
            {
                isDragging = false;

                // Snap to the predetermined position
                transform.position = new Vector3(snapPosition.x, snapPosition.y, fixedZPosition);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            // Move the capsule to follow the mouse cursor (on the X-Y plane)
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, fixedZPosition)); // Constrain to X-Y plane
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);

                // Update position, keeping Z position fixed
                transform.position = new Vector3(worldPoint.x, worldPoint.y, fixedZPosition);
            }
        }
    }
}