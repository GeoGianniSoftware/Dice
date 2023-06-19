using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBoss : MonoBehaviour
{

    PlayerController PC;
    public GameObject baseBone;
    public GameObject arm1Bone;
    public GameObject arm2Bone;

    public float arm1MaxRot = 90f;
    public float maxDistance = 20f;

    SkinnedMeshRenderer meshRenderer;
    MeshCollider collider;

    public float damping;
    // Start is called before the first frame update
    void Start()
    {
        PC = FindObjectOfType<PlayerController>();
        meshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        collider = transform.GetChild(1).GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        UpdateCollider();

    }

    public void UpdateCollider() {
        Mesh colliderMesh = new Mesh();

        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }

    void TrackPlayer() {
        Vector3 lookPos = baseBone.transform.position - PC.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        baseBone.transform.rotation = Quaternion.Slerp(baseBone.transform.rotation, rotation, Time.deltaTime * damping);

        Vector3 playerPos = PC.transform.position;
        playerPos.y = transform.position.y;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        print(distanceToPlayer);
        float playerPercentage = distanceToPlayer / maxDistance;

        playerPercentage -= .25f;
        if (playerPercentage < .25f) {
            playerPercentage = .25f;
        }else if(playerPercentage > 1) {
            playerPercentage = 1;
        }

        var arm1Rot = Quaternion.Euler(0, (playerPercentage*4)* maxDistance, -90);

        arm1Bone.transform.localRotation = Quaternion.Slerp(arm1Bone.transform.localRotation, arm1Rot, Time.deltaTime * damping);

        Vector3 lookPosLast = arm2Bone.transform.position - PC.transform.position;
        lookPosLast.y = 0;
        lookPosLast.z = -45;
        var arm2Rot = Quaternion.Euler(20, 0, -90);

        arm2Bone.transform.localRotation = Quaternion.Slerp(arm2Bone.transform.localRotation, arm2Rot, Time.deltaTime * damping);

    }
}
