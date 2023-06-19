using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour
{
    Rigidbody RB;
    public float force = 5;
    public int currentValue;
    public Canvas diceCanvas;
    public Text diceResult;
    float canvasHeight;
    public bool ready = false;

    // Start is called before the first frame update
    void Start() {
        RB = GetComponent<Rigidbody>();
        canvasHeight = diceCanvas.transform.localScale.y;
        diceCanvas.transform.localScale = new Vector3(diceCanvas.transform.localScale.x, 0, diceCanvas.transform.localScale.z);
        Shuffle();
    }
    private void Awake() {
        Start();
    }

    // Update is called once per frame
    public void Shuffle() {
        Vector3 temp = new Vector3();
        temp.x = Random.Range(-364, 364);
        temp.y = Random.Range(-364, 364);
        temp.z = Random.Range(-364, 364);
        transform.rotation = Quaternion.Euler(temp);
    }

    public void Throw(Transform thrower) {
        Shuffle();

        float multiplier = Random.Range(0.85f, 1.85f);
        float m1, m2, m3;
        m1 = Random.Range(0.25f, 1.0f);
        m2 = Random.Range(0.25f, 1.0f);
        m3 = Random.Range(0.25f, 1.0f);
        float finForce = force * multiplier;

        RB.isKinematic = false;
        RB.AddForce(thrower.forward * finForce / 2);
        RB.AddForce(thrower.up * finForce / 2);

        RB.AddTorque(transform.right * finForce*m1);
        RB.AddTorque(transform.forward * finForce*m2);
        RB.AddTorque(transform.up * finForce*m3);
    }

    void Update() {
        if (RB.velocity.magnitude > .05f) {
            ready = false;
            currentValue = -1;
            diceCanvas.transform.localScale = new Vector3(diceCanvas.transform.localScale.x, 0, diceCanvas.transform.localScale.z);
            diceCanvas.enabled = false;
        }
        else {
            
            currentValue = returnDiceFace();
            Vector3 temp = new Vector3(diceCanvas.transform.localScale.x, canvasHeight, diceCanvas.transform.localScale.z);
            if (!ready) {
                diceCanvas.transform.position = transform.position + (Vector3.up * (3 + transform.localScale.y));
                diceResult.text = "" + currentValue;
                diceCanvas.enabled = true;
            }
            diceCanvas.transform.localScale = Vector3.Lerp(diceCanvas.transform.localScale, temp, .01f);
           
            diceCanvas.transform.LookAt(Camera.main.transform);
            
            ready = true;
        }


        if (Input.GetKeyDown(KeyCode.Space)) {

            Throw(Camera.main.transform);

        }
    }



    int returnDiceFace() {
        Vector3[] directions = { transform.up, -transform.up, -transform.right, transform.right, transform.forward, -transform.forward };
        int[] diceValue = { 2, 5, 6, 1, 3, 4 };
        float min = float.MaxValue;
        int index = -1;
        for (int i = 0; i < 6; i++) {


            float ang = Vector3.Angle(directions[i], Vector3.up);
            if (ang < min) {
                min = ang;
                index = i;
            }
        }
        return diceValue[index];
    }
}
