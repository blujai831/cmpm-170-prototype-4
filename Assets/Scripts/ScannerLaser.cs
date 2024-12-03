using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScannerLaser : MonoBehaviour
{
    [SerializeField] float size;
    private GameObject triangleObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private List<GameObject> correctlyAlignedBarcodes;
    private List<GameObject> successfullyScannedBarcodes;


    // Start is called before the first frame update
    void Start()
    {
        // Create a new GameObject for the triangle
        triangleObject = new GameObject("TriangleScanner");

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Define the vertices (equilateral triangle)
        float height = Mathf.Sqrt(3) / 2 * size; // Height of equilateral triangle

        Vector3[] vertices = new Vector3[]
        {
            Vector3.zero, // Point of the triangle
            new Vector3(-size / 2, -height, 0), // Bottom-left
            new Vector3(size / 2, -height, 0),  // Bottom-right
        };

        Vector3[] colliderVertices = new Vector3[] {
            Vector3.zero,
            new Vector3(-size/2, 0, height),
            new Vector3(size/2, 0, height),
            Vector3.up*-0.1f,
            new Vector3(-size/2, -0.1f, height),
            new Vector3(size/2, -0.1f, height)
        };

        // Define the triangle indices
        int[] triangles = new int[]
        {
            0, 1, 2
        };

        int[] colliderTriangles = new int[] {
            0, 1, 2, 3, 4, 5
        };

        // Apply to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Attach to MeshFilter and Renderer
        meshFilter = triangleObject.AddComponent<MeshFilter>();
        meshRenderer = triangleObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        meshFilter.mesh = mesh;

        meshCollider.sharedMesh = new Mesh();
        meshCollider.sharedMesh.vertices = colliderVertices;
        meshCollider.sharedMesh.triangles = colliderTriangles;
        meshCollider.sharedMesh.RecalculateNormals();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;

        // Assign red material
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = Color.red;
        meshRenderer.material = material;

        // Parent the triangle to the capsule
        triangleObject.transform.SetParent(transform);

        correctlyAlignedBarcodes = new List<GameObject>();
        successfullyScannedBarcodes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Position the triangle slightly in front of the capsule
        triangleObject.transform.localPosition = new Vector3(0, 0, 0); // Adjust distance as needed

        // Apply rotation offset so the triangle points slightly downward or at an angle
        Quaternion rotationOffset = Quaternion.Euler(-90, 0, 0); // Rotate -90 degrees on X-axis
        triangleObject.transform.rotation = transform.rotation * rotationOffset;
    }

    bool IsCorrectlyAlignedAndUnscannedBarcode(GameObject gobj) {
        return gobj.CompareTag("Barcode") &&
            !correctlyAlignedBarcodes.Contains(gobj) &&
            !successfullyScannedBarcodes.Contains(gobj) &&
            BarcodeCorrectlyAligned(gobj);
    }

    void OnTriggerEnter(Collider other) {
        if (IsCorrectlyAlignedAndUnscannedBarcode(other.gameObject)) {
            correctlyAlignedBarcodes.Add(other.gameObject);
            if (AllBarcodesAligned()) {
                FlushBarcodes();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        correctlyAlignedBarcodes.Remove(other.gameObject);
    }

    bool AllBarcodesAligned() {
        foreach (GameObject gobj in GameObject.FindGameObjectsWithTag("Barcode")) {
            if (successfullyScannedBarcodes.Contains(gobj)) {
                continue;
            } else if (!correctlyAlignedBarcodes.Contains(gobj)) {
                return false;
            }
        }
        return true;
    }

    void FlushBarcodes() {
        foreach (GameObject gobj in correctlyAlignedBarcodes) {
            successfullyScannedBarcodes.Add(gobj);
        }
        correctlyAlignedBarcodes.Clear();
        TurnGreen();
        Invoke("TurnRed", 0.5f);
    }

    void TurnGreen() {
        meshRenderer.material.color = Color.green;
    }

    void TurnRed() {
        meshRenderer.material.color = Color.red;
    }

    bool BarcodeCorrectlyAligned(GameObject gobj) {
        var forwardAlignment = Vector3.Dot(gobj.transform.forward, -Camera.main.transform.forward);
        var rollAlignment = Math.Abs(Vector3.Dot(
            Vector3.ProjectOnPlane(gobj.transform.right, Camera.main.transform.forward).normalized,
            Camera.main.transform.right
        ));
        //Debug.Log($"{forwardAlignment} {rollAlignment}");
        return forwardAlignment > 0.0f && rollAlignment > 0.97f;
    }
}
