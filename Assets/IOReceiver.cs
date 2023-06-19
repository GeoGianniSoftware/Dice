using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOReceiver : MonoBehaviour
{
    public bool activated;
    public bool oneTime;


    public void SetActivateStatus(bool status) {
        activated = status;

    }

    public void Activate() {
        activated = true;
       
    }

    public void Deactivate() {
        if (oneTime && activated)
            return;
        activated = false;
    }
}
