using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScannerLaser : MonoBehaviour
{
    private List<GameObject> correctlyAlignedBarcodes;
    private List<GameObject> successfullyScannedBarcodes;
    [SerializeField] private Score scoreScript;
    [SerializeField] private Timer timerScript;
    [SerializeField] private GrocerySpawner spawnerScript;

    private bool customerBlocking;

    // Start is called before the first frame update
    void Start()
    {
        correctlyAlignedBarcodes = new List<GameObject>();
        successfullyScannedBarcodes = new List<GameObject>();
        customerBlocking = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool IsCorrectlyAlignedAndUnscannedBarcode(GameObject gobj) {
        return gobj.CompareTag("Barcode") &&
            !correctlyAlignedBarcodes.Contains(gobj) &&
            !successfullyScannedBarcodes.Contains(gobj) &&
            BarcodeCorrectlyAligned(gobj);
    }

    void OnTriggerEnter(Collider other)
    {

        // If customer hit, turn off laser
        if (other.gameObject.CompareTag("Customer"))
        {
            TurnClear();
            customerBlocking = true;
        }


        // if barcode hit, add barcode to list
        if (IsCorrectlyAlignedAndUnscannedBarcode(other.gameObject))
        {
            correctlyAlignedBarcodes.Add(other.gameObject);
            if (AllBarcodesAligned() && !customerBlocking)
            {
                FlushBarcodes();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        correctlyAlignedBarcodes.Remove(other.gameObject);

        if (other.gameObject.CompareTag("Customer"))
        {
            TurnRed();
            customerBlocking = false;
        }
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
        int numBarcodes = 0;
        foreach (GameObject gobj in correctlyAlignedBarcodes) {
            successfullyScannedBarcodes.Add(gobj);
            numBarcodes++;
        }
        correctlyAlignedBarcodes.Clear();
        TurnGreen();
        scoreScript.UpdateScore(numBarcodes);
        Invoke("TurnRed", 0.5f);
        DestroyGroceries(); //despawn scanned groceries, can later change to be moved/animated
        timerScript.ResetTimer(); //successful so reset timer
        spawnerScript.SpawnGroceries(1.0f);
    }

    void TurnGreen() {
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
    }

    void TurnRed() {
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
    }

    void TurnClear()
    {
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.clear);
    }

    bool BarcodeCorrectlyAligned(GameObject gobj) {
        var forwardAlignment = Vector3.Dot(gobj.transform.forward, -Camera.main.transform.forward);
        var rollAlignment = Math.Abs(Vector3.Dot(
            Vector3.ProjectOnPlane(gobj.transform.right, Camera.main.transform.forward).normalized,
            Camera.main.transform.right
        ));
        //Debug.Log($"{forwardAlignment} {rollAlignment}");
        return forwardAlignment > 0.0f && rollAlignment > 0.87f;
    }

    void DestroyGroceries()
    {
        int numGroceries = successfullyScannedBarcodes.Count;
        for (int i = 0; i < numGroceries; i++)
        {
            Destroy(successfullyScannedBarcodes[0].gameObject.transform.parent.gameObject);
            successfullyScannedBarcodes.RemoveAt(0);
        }
        successfullyScannedBarcodes.Clear();
    }
}
