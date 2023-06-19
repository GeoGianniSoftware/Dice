using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Projectile", menuName ="Projectiles/Basic", order =1)]
public class Projectile : ScriptableObject
{
    [ReadOnly]
    public Weapon source;
    public bool destroyOnHit;
    public int hitCount = 1;
    public GameObject projectilePrefab;
    public float glideFactor;
    public float scanDistance = .5f;
    public bool faceVelocity;


}
