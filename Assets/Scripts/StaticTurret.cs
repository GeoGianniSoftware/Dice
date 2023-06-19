using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTurret : BasicRangedEnemy
{
    public float engagmentAngle;
    public float engagementRange;
    public float moveSpeed;
    public PlayerController PC;
    public float playerAngle;
    public Transform head;
    public Transform body;

    public Transform projectileSpawn1;
    public Transform projectileSpawn2;
    float targetDist;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        weapon.targetedFire = false;

        if (FindObjectOfType<PlayerController>() != null) {
            PC = FindObjectOfType<PlayerController>();
        }
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if(PC != null && canFireOnPlayer()) {
            float fireOffset =  targetDist/ engagementRange;
            weapon.setFireOffset(fireOffset);
            if (idle <= 0) {

                Fire();
                SwitchBarrel();
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0,-playerAngle,0)), .18f);

            Vector3 moveDir = PC.getMoveDirection();
            moveDir.y = 0;

            Vector3 barrelLookAt = PC.senses.transform.position + (moveDir/2);

            projectileSpawn1.LookAt(barrelLookAt + randomOffset(-.5f));
            projectileSpawn2.LookAt(barrelLookAt+ randomOffset(.5f));
        }
        else {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, .18f);
        }
        Tick();

    }

    Vector3 randomOffset(float mult) {
        Vector3 temp = new Vector3(Random.Range(0, 1f), Random.Range(-.05f, .05f), Random.Range(0, 1f));
        temp *= mult;
        return temp;
    }

    int index = 0;
    void SwitchBarrel() {
        switch (index) {
            case 0:
                weapon.setProjectileSpawn(projectileSpawn1);
                index = 1;
                break;
            case 1:
                weapon.setProjectileSpawn(projectileSpawn2);
                index = 0;
                break;
        }
        
    }

    bool canFireOnPlayer() {
        Vector3 p1 = transform.position;
        Vector3 p2 = PC.transform.position;

        float angleToTarget = (Mathf.Atan2(p2.z - p1.z, p2.x - p1.x) * 180 / Mathf.PI) - 90;
        playerAngle = angleToTarget;
        if (Mathf.Abs(playerAngle)<= engagmentAngle) {
            float dist = Vector3.Distance(transform.position, PC.transform.position);
            targetDist = dist;
            if (dist <= engagementRange)
                return true;
        }
        return false;
    }
}
