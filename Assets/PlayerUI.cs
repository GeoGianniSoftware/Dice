using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    public Text ActivateText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowActivateText(bool state) {
        ActivateText.enabled = state;
    }

    public void SetActivateText(string textToSet) {
        ActivateText.text = textToSet;
    }
}
