using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOKeypad : IOInput
{
   
    public Material lockedMat;
    public Material unlockedMat;

    public List<GameObject> lights;
    public List<GameObject> activeLight;

    public List<Collectable> keys;


    bool allUnlocked = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        allUnlocked = true;
        foreach(Collectable c in keys) {
            if (!c.collected) {

                allUnlocked = false;
                

            }
        }
        ToggleLights(!allUnlocked);
    }

    private void OnMouseEnter() {
        PlayerUI UI = FindObjectOfType<PlayerUI>();
        UI.ShowActivateText(true);
        if (allUnlocked) {
            UI.SetActivateText("Press 'E'\n to Activate");
        }
        else {
            UI.SetActivateText("Find Keycard");
        }



        
    }

    private void OnMouseOver() {
        if (Input.GetKeyDown(KeyCode.E) && allUnlocked) {
            if (activated) {
                Deactivate();
            }
            else {
                Activate();
            }
            
        }
        ToggleActiveLights(!activated);
    }

    private void OnMouseExit() {
        FindObjectOfType<PlayerUI>().ShowActivateText(false);
        //ToggleActiveLights(true);
    }

    void ToggleLights(bool locked) {
        if (locked) {
            foreach(GameObject light in lights) {
                light.GetComponent<MeshRenderer>().sharedMaterial = lockedMat;

            }
        }
        else {
            foreach (GameObject light in lights) {
                light.GetComponent<MeshRenderer>().sharedMaterial = unlockedMat;

            }
        }
    }

    void ToggleActiveLights(bool locked) {
        if (locked) {
            foreach (GameObject light in activeLight) {
                light.GetComponent<MeshRenderer>().sharedMaterial = lockedMat;

            }
        }
        else {
            foreach (GameObject light in activeLight) {
                light.GetComponent<MeshRenderer>().sharedMaterial = unlockedMat;

            }
        }
    }


}
