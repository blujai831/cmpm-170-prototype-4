using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrocerySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> groceries;
    [SerializeField] private Score scoreScript;

    public void SpawnGroceries(float when = 0.0f) {
        Invoke("SpawnGroceriesImmediately", when);
    }

    private void SpawnGroceriesImmediately() {
        for (int count = (int) UnityEngine.Random.Range(1.0f, 5.0f); count > 0; --count) {
            var posn = new Vector3(0.0f, 12.0f + (float) count*3.0f, 0.0f);
            var i = ((int) (UnityEngine.Random.Range(0.0f, (float) groceries.Count))) % groceries.Count;
            var grocery = Instantiate(groceries[i], posn, Quaternion.identity);
            var dropPenalty = grocery.GetComponent<DropPenalty>();
            dropPenalty.scoreScript = scoreScript;
            dropPenalty.spawnerScript = this;
        }
    }

    public void CheckForNoGroceriesLater(float when = 0.1f) {
        Invoke("CheckForNoGroceriesImmediately", when);
    }

    private void CheckForNoGroceriesImmediately() {
        if (GameObject.FindGameObjectsWithTag("Barcode").Length == 0) {
            SpawnGroceries();
        }
    }
}
