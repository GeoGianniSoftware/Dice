using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public Vector3 acceleration;
    public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            PlayerController PC = other.GetComponent<PlayerController>();
            
            if(PC.getMoveDirection().y < maxSpeed) {
                PC.addMoveDirection(acceleration);
            }
            if (PC.getMoveDirection().y < 0) {
                PC.addMoveDirection(new Vector3(0,-PC.getMoveDirection().y,0));
            }
        }
    }

   
}
