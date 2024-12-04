using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customer;
    [SerializeField] private float frequency;
    [SerializeField] private float walkSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.Range(0.0f, 1.0f) <= frequency) {
            var instance = Instantiate(customer);
            instance.GetComponent<Customer>().walkSpeed = walkSpeed;
        }
    }
}
