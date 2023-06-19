using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMeleeEnemy : BasicEnemy
{
    NavMeshAgent NMA;
    public float moveSpeed;


    public float engagementRange;
    public float wanderRange;

    public RangedWeapon weapon;
    public Transform projectileSpawnPos;
    PlayerController PC;
    public float skinLength = .5f;
    public bool frontGrounded;
    bool middleGrounded;

    Vector3 storedVelocity;
    public float stunInvuln = 0f;

    // Start is called before the first frame update
    public virtual void Start() {
        Initialize();
        NMA = GetComponent<NavMeshAgent>();
        if(GameObject.FindObjectOfType<PlayerController>() != null) {
            PC = GameObject.FindObjectOfType<PlayerController>();
        }
        if(weapon != null) {
            weapon = Instantiate(weapon);
            weapon.setProjectileSpawn(projectileSpawnPos);
        }
    }

    // Update is called once per frame

    private void OnDrawGizmos() {
        Gizmos.DrawRay(new Ray(transform.position + ((transform.forward * 1.2f) * transform.localScale.z), Vector3.down));
        }
    public virtual void Update() {
        transform.LookAt(transform.position + NMA.velocity);
        stunInvuln -= Time.deltaTime;
        Fire();
        if (Input.GetKeyDown(KeyCode.P)) {
            TakeDamage(5);
        }
 



        if (PC != null && NMA.isOnNavMesh) {
            print("ding");
            NMA.SetDestination(PC.transform.position);
            NMA.speed = moveSpeed;
        }


        //End Loop
        Tick();
    }

  

    public void Fire() {
        if (idle <= 0 && weapon != null) {
            weapon.Attack();

            Idle(weapon.attackSpeed);
        }
    }
}
