using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public bool live = false;
    public Projectile source;
    public Rigidbody RB;
    Collider COL;
    float hitDelay = .1f;
    float delay;
    public bool faceVelocity;

    bool playerProjectile = false;
    float forceOffset = 1;

    public void setForceOffset(float set) {
        forceOffset = set;
    }
    public void Initialize(Projectile s, RangedWeapon w)
    {
        COL = GetComponent<Collider>();
        RB = GetComponent<Rigidbody>();
        source = Instantiate(s);
        source.source = w;
        faceVelocity = s.faceVelocity;

        if (w.isPlayerWeapon()) {
            playerProjectile = true;
            this.gameObject.layer = 9;


        }
        if (source != null) {
            RB.AddForce(transform.forward * w.getSpeed()*forceOffset, ForceMode.VelocityChange);
        }
        live = true;
    }



    void CollisionRoutine(Collider col) {
        
        if (col != null && live && delay <= 0) {
            if(col.GetComponent<PlayerController>())
            print(source.source + " has hit: " + col.transform.name);

            source.glideFactor = 0;
            DealDamage(col.gameObject);
            source.hitCount--;
            if (source.hitCount <= 0) {
                if (source.destroyOnHit) {
                    foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>()) {
                        if (p.IsAlive())
                            p.transform.SetParent(null);
                    }

                    Destroy(gameObject);
                }

                live = false;
            }
            delay = hitDelay;
        }
    }

    private void OnCollisionEnter(Collision collision) {

        CollisionRoutine(collision.collider);
    }

    bool friendlyFire(GameObject obj) {
        return !(playerProjectile && obj.transform.GetComponent<PlayerController>());
    }

    private void Update() {
        if (faceVelocity)
            transform.GetChild(0).LookAt(transform.position+RB.velocity);
        delay -= Time.deltaTime;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, source.scanDistance)){
            if(hit.collider != null && friendlyFire(hit.transform.gameObject)) {
                CollisionRoutine(hit.collider);
            }
        }else if(Physics.OverlapSphere(transform.position, .5f)!= null) {
            Collider[] cols = Physics.OverlapSphere(transform.position, .5f);
            foreach(Collider col in cols) {
                if(col != COL && friendlyFire(col.transform.gameObject)) {

                    CollisionRoutine(col);
                }
            }
        }
    }
    private void FixedUpdate() {
        if (RB != null) {
            RB.AddForce(Vector3.down * (Physics.gravity.y * source.glideFactor));
        }
    }


    void DealDamage(GameObject target) {
        Vector3 hitPoint = target.transform.position - transform.position;

        if (target.GetComponent<BasicEnemy>())
            target.GetComponent<BasicEnemy>().AddImpact(hitPoint*3);
        target.SendMessage("TakeDamage", source.source.getDamage(), SendMessageOptions.DontRequireReceiver);
    }
}
