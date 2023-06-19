using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOButton : IOInput
{
    Vector3 buttonPosVisual;
    GameObject buttonTop;
    public bool playerOnly = true;
    public Material inactiveMat, activeMat;

    // Start is called before the first frame update
    void Start()
    {
        buttonTop = transform.GetChild(0).gameObject;
        if(connectedObjects.Count > 0) {
            foreach (GameObject g in connectedObjects) {
                if (g == null)
                    return;
                g.SetActive(false);
            }
        }
        
        buttonPosVisual = buttonTop.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated) {
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, buttonPosVisual, .98f);
            buttonTop.GetComponent<MeshRenderer>().sharedMaterial = inactiveMat;
        }
        else {
            buttonTop.transform.position = Vector3.Lerp(buttonTop.transform.position, buttonPosVisual+(Vector3.down/3), .98f);
            buttonTop.GetComponent<MeshRenderer>().sharedMaterial = activeMat;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(playerOnly && other.tag != "Player") {
            return;
        }
        Activate();
    }
    private void OnTriggerExit(Collider other) {
        if (playerOnly && other.tag != "Player") {
            return;
        }
        Deactivate();
    }

}
