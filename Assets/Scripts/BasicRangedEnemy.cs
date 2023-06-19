using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemy : BasicEnemy
{

    public RangedWeapon weapon;
    public Transform projectileSpawnPos;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Initialize();

        weapon = Instantiate(weapon);


        weapon.setProjectileSpawn(projectileSpawnPos);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Fire();
        if (Input.GetKeyDown(KeyCode.P)) {
            TakeDamage(5);
        }
        //End Loop
        Tick();
    }

   public void Fire() {
        if (idle <= 0) {
            weapon.Attack();
            
            Idle(weapon.attackSpeed);
        }
    }
}
