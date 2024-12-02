using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerLaser : MonoBehaviour
{
    [SerializeField] float size;
    private GameObject triangleObject;



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

        // Define the triangle indices
        int[] triangles = new int[]
        {
            0, 1, 2
        };

        // Apply to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Attach to MeshFilter and Renderer
        MeshFilter meshFilter = triangleObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = triangleObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;

        // Assign red material
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = Color.red;
        meshRenderer.material = material;

        // Parent the triangle to the capsule
        triangleObject.transform.SetParent(transform);
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
}
