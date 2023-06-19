using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicEnemy : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;
    public EnemyUI UI;
    
    public float idle;
    public bool invulnerable;

    Rigidbody RB;
    // Start is called before the first frame update
    public void Initialize()
    {
        RB = GetComponent<Rigidbody>();
        UI = GetComponentInChildren<EnemyUI>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddImpact(Vector3 impact) {
        RB.AddForce(impact, ForceMode.Impulse);
    }

    public void Idle(float time) {
        idle = time;
    }

    public void Tick() {
        if (currentHealth <= 0 && !invulnerable || transform.position.y < -10f) {
            Die();
            return;
        }

        idle -= Time.deltaTime;
    }

    void Die() {
        Destroy(gameObject);
    }

    public void TakeDamage(int amt) {
        currentHealth -= amt;
        UI.setHealthbarPercentage((float)((float)currentHealth / (float)maxHealth));
    }
}
