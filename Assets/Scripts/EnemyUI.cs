using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image healthbar;
    public Image healthbarBackground;
    Canvas CANVAS;

    // Start is called before the first frame update
    void Start()
    {

        CANVAS = GetComponent<Canvas>();
        CANVAS.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main != null) {
            transform.LookAt(Camera.main.transform);
        }
        if(healthbar != null && healthbarBackground != null) {
            if (healthbar.fillAmount >= 1) {
                healthbarBackground.enabled = false;
            }
            else {
                healthbarBackground.enabled = true;
            }
        }
    }

    public void setHealthbarPercentage(float percent) {
        healthbar.fillAmount = percent;
    }
}
