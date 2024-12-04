using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPenalty : MonoBehaviour
{
    [SerializeField] public Score scoreScript;
    [SerializeField] public GrocerySpawner spawnerScript;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1) {
            scoreScript.UpdateScore(-1);
            Destroy(gameObject);
            spawnerScript.CheckForNoGroceriesLater();
        }
    }
}
