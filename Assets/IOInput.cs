using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOInput : MonoBehaviour
{
    public bool activated;
    public bool oneTime;

    public List<GameObject> connectedObjects;

    public List<IOReceiver> connectedReceivers;



    public void Activate() {
        activated = true;
        UpdateCollection();
    }

    public void Deactivate() {
        if (oneTime && activated)
            return;
        activated = false;
        UpdateCollection();
    }

    void UpdateCollection() {
        foreach (GameObject g in connectedObjects) {
            g.SetActive(activated);
        }
        foreach (IOReceiver R in connectedReceivers) {
            R.SetActivateStatus(activated);
        }
    }
}
