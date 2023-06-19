using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IODrawBridge : IOReceiver
{
    public OffMeshLink[] bridgeLinks;
    // Start is called before the first frame update
    public GameObject left;
    public GameObject right;
    public float closeSpeed = 1;
    Quaternion startRight;
    Quaternion startLeft;
    float percentageComplete;
    void Start()
    {
        startLeft = left.transform.rotation;
        startRight = right.transform.rotation;

        oneTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && percentageComplete < 1) {
            if(bridgeLinks.Length > 0) {
                foreach(OffMeshLink link in bridgeLinks) {
                    link.activated = true;
                }
            }
            percentageComplete += closeSpeed * Time.deltaTime;

            left.transform.rotation = Quaternion.Lerp(startLeft, Quaternion.Euler(0, 0, 0), percentageComplete);

            right.transform.rotation = Quaternion.Lerp(startRight, Quaternion.Euler(0, 0, 0), percentageComplete);
        }
    }
}
