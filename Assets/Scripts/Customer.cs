using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float walkSpeed;
    private Vector3 walkDirection;
    void Start() {
        if (Random.Range(0.0f, 1.0f) <= 0.5f) {
            walkDirection = Vector3.right;
        } else {
            walkDirection = Vector3.left;
        }
        transform.position = -walkDirection*15.0f - 5.0f*Vector3.forward;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.material.color = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            1.0f
        );
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += walkDirection*walkSpeed*Time.deltaTime;
        if (transform.position.x > 15.0f || transform.position.x < -15.0f) {
            Destroy(gameObject);
        }
    }
}
