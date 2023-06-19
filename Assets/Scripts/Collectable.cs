using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool collected;
    public float rotationSpeed = 12f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter() {
        PlayerUI UI = FindObjectOfType<PlayerUI>();
        
        if (!collected) {
            UI.ShowActivateText(true);
            UI.SetActivateText("Press 'E'\n to Collect");
        }
        else {
            UI.ShowActivateText(false);
        }




    }

    private void OnMouseOver() {
        PlayerUI UI = FindObjectOfType<PlayerUI>();
        if (Input.GetKeyDown(KeyCode.E)){
            collected = true;
            UI.ShowActivateText(false);
        }
    }

    private void OnMouseExit() {
        PlayerUI UI = FindObjectOfType<PlayerUI>();
        UI.ShowActivateText(false);
    }

    float rot = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rot + Time.deltaTime * rotationSpeed);

        if (collected) {
            if(GetComponent<MeshRenderer>() != null) {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() != null && collected == false) {
            print("Collected Key");
            collected = true;
        }
    }
}
