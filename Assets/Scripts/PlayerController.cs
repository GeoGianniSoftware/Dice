using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script Refs
    public PlayerSenses senses;
    CharacterController characterController;
    [System.NonSerialized]
    public Collider playerCollider;



    #region #Variables#: Movement
    [Header("Movement")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float fallMultiplier = 2.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float climbRange = 1.5f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float jumpBuff;
    float speedBuff;
    float jtime;
    [HideInInspector]
    public bool canMove = true;
    float coyoteTime = .2f;
    bool canJump;
    Vector3 moveDirection = Vector3.zero;
    Vector2 sidewaysMoveOffset = Vector3.zero;
    float rotationX = 0;
    //Variables #Movement# end
    #endregion

    [Header("Combat")]
    public List<Weapon> weapons;
    [ReadOnly]
    public Weapon currentWeapon;
    public BaseAbility ability;
    public List<Buff> currentBuffs = new List<Buff>();
    float attackIdle;
    int damageBuff;

    float floorAngle;
    Vector3 hitNormal;
    bool isGrounded;
    public float slideSpeed;




    #region UnityFunctions
    void Start() {
        characterController = GetComponent<CharacterController>();

        playerCollider = GetComponent<Collider>();
        senses = FindObjectOfType<PlayerSenses>();

        Physics.IgnoreLayerCollision(8, 9, true);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<MeshRenderer>().enabled = false;
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
            hitNormal = hit.normal;


    }


    void Update() {
        //transform.Rotate(Vector3.up, senses.transform.rotation.y);
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ability.Initialize(this);
            print("Casting: " + ability.name);
            ability.Cast();
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            EquipWeapon(weapons[0]);
        }
        if (Input.GetMouseButtonDown(0)) {
            Attack();
        }
        sidewaysMoveOffset = Vector2.Lerp(sidewaysMoveOffset, Vector2.zero, .18f);
        attackIdle -= Time.deltaTime;

        BuffLoop();
        Movement();


    }
    #endregion

    #region Movement
    public void setMoveDirection(Vector3 force) {
        moveDirection = force;
    }

    public Vector3 getMoveDirection() {
       return moveDirection;
    }

    public void addMoveDirection(Vector3 force) {
        moveDirection += force;
        sidewaysMoveOffset = new Vector2(force.x, force.z);
    }

    void MouseAiming() {
        //Slip Check

        if (!isGrounded) {
            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * slideSpeed;
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * slideSpeed;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        isGrounded = (Vector3.Angle(Vector3.up, hitNormal) <= characterController.slopeLimit);
        

        if (canMove) {
            float y = Input.GetAxis("Mouse X") * lookSpeed;
            transform.Rotate(Vector3.up, y);
        }
    }

    bool HeadCheck() {
        Ray ray = new Ray(senses.transform.position, Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, .5f)) {
            if (hit.collider != null) {
                return true;
            }
        }
        return false;
    }

    void Movement() {
        MouseAiming();
        if (HeadCheck() && moveDirection.y > 0) {
            moveDirection.y = 0;
        }
        jtime -= Time.deltaTime;

        if (characterController.isGrounded) {
            jtime = coyoteTime;

        }
        if (jtime > 0 || characterController.isGrounded) {
            canJump = true;
        }
        else {
            canJump = false;
        }


        //Look Direction Check
        bool lookingUp = false;
        if(Vector3.Dot(senses.transform.forward, Vector3.up) > .866f) {
            lookingUp = true;
            print("Looking Up");
        }


        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.forward;
        Vector3 right = Camera.main.transform.right;
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && canJump;
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));
        inputVector = inputVector.normalized;
        float curSpeedX = canMove ? (isRunning ? runningSpeed + speedBuff : walkingSpeed + speedBuff) *  inputVector.x: 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed + speedBuff : walkingSpeed + speedBuff) *  inputVector.z: 0;
        curSpeedX += sidewaysMoveOffset.x;
        curSpeedY += sidewaysMoveOffset.y;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButtonDown("Jump") && canJump) {
            if (canMove) {
                moveDirection.y = jumpSpeed + jumpBuff;
            }


        }
        else {

            moveDirection.y = movementDirectionY;
        }



        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        float gravSmooth = 0.1f;
        if (!characterController.isGrounded && !canJump) {
            float finMultiplier = fallMultiplier;
            if (moveDirection.y > 0) {
                gravSmooth = 1f;
                finMultiplier = 1;
            }
            else
             gravSmooth = Mathf.Lerp(gravSmooth, 1f, .5f);

            moveDirection.y -= gravity * gravSmooth * finMultiplier * Time.deltaTime;
        }



        
        
    }

    #endregion

    void Attack() {
        if (!currentWeapon || !currentWeapon.initialized || attackIdle > 0)
            return;
        
        if(currentWeapon.initialized) {
            currentWeapon.Attack(damageBuff);
            attackIdle += currentWeapon.attackSpeed;
        }
    }

    void EquipWeapon(Weapon weaponToEquip) {
        currentWeapon = Instantiate(weaponToEquip);
        currentWeapon.Initialize(this.gameObject);
        if(currentWeapon.weaponType == WeaponType.RANGED) {
            ((RangedWeapon)currentWeapon).setProjectileSpawn(senses.projectileSpawn);
        }
    }


    #region Buffs
    public void AddBuff(Buff buffToAdd) {
        currentBuffs.Add(Instantiate(buffToAdd));
        if(buffToAdd.buffType == BuffType.JUMP) {
            jumpBuff += buffToAdd.amt;
        }
        else if (buffToAdd.buffType == BuffType.SPEED) {
            speedBuff += buffToAdd.amt;
        }
        else if (buffToAdd.buffType == BuffType.DAMAGE) {
            damageBuff += (int)buffToAdd.amt;
        }
    }

    public void RemoveBuff(Buff buffToRemove) {
        if (currentBuffs.Contains(buffToRemove)) {
            if(buffToRemove.buffType == BuffType.JUMP) {
                jumpBuff -= buffToRemove.amt;
            }
            else if (buffToRemove.buffType == BuffType.SPEED) {
                speedBuff -= buffToRemove.amt;
            }
            else if (buffToRemove.buffType == BuffType.DAMAGE) {
                damageBuff -= (int)buffToRemove.amt;
            }

            currentBuffs.Remove(buffToRemove);
        }
        
    }

    void BuffLoop() {
        List<Buff> buffsToRemove = new List<Buff>();

        foreach (Buff b in currentBuffs) {
            
            b.Tick(Time.deltaTime);
            if (b.lifetime <= 0) {
                //Buff Ends
                buffsToRemove.Add(b);
            }
        }

        foreach (Buff b in buffsToRemove) {
            if (currentBuffs.Contains(b)) {
                RemoveBuff(b);
            }
        }
    }

    #endregion

    

    


}
