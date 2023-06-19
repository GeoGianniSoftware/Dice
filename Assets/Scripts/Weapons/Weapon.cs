using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Base/Basic Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [ReadOnly]
    public bool initialized = false;
    public WeaponType weaponType;
    public bool randomDamage;
    public int damageMin, damageMax;
    public float range;
    public float attackSpeed;
    [ReadOnly]
    public GameObject owner;

    public bool isPlayerWeapon() {
        
        if (owner != null && owner.GetComponent<PlayerController>()) {
            return true;
        }
        return false;
        
    }

    private void OnValidate() {
        Reset();
    }

    public virtual void Reset() {
        initialized = false;
    }

    public virtual void Initialize(GameObject setOwner) {
        owner = setOwner;

        initialized = true;
    }

    public int getDamage() {
        if (randomDamage) {
            int rand = Random.Range(damageMin, damageMax);
            return rand;
        }
        else {
            return damageMin;
        }
        
    }

    public virtual void Attack() {

    }

    public virtual void Attack(int attackBuff) {

    }
}

public enum WeaponType
{
    MELEE,
    RANGED
}
