using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Energy))]
public class Robot : Mirror.NetworkBehaviour {

    [Mirror.SyncVar]
    private float health = 100;
    [Mirror.SyncVar]
    private float energy = 100;
    [SerializeField]
    private float energyDecayRate = 0.5f;

    [Header("Components")]
    private Animator animator;
    private Rigidbody rigidBody;

    [Header("Movement")]
    private Controller controller; //controller (AI or human)

    //    [Header("Firing")]
    //    public KeyCode shootKey = KeyCode.Space;
    //    public GameObject projectilePrefab;
    //    public Transform projectileMount;

    public Transform aimTarget;
    private Vector3 originalAimPos;
    private SpriteRenderer aimSprite;
    private float verticalAim;
    [SerializeField] private float verticalSensitivity = 10;


    [Header("Stats")]
    [SerializeField] private float maxAim = 2f;
    [SerializeField] private float minAim = -2f;
    [SerializeField] private float maxRange = 100;
    //health and energy
    private bool dead = false;

    private Weapon weapon;


    private void Awake() {
        controller = GetComponent<Controller>();

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        weapon = GetComponent<Weapon>();
    }

    private void Start() {
        health = GetComponent<Health>().maxHealth;
        energy = GetComponent<Energy>().maxEnergy;
        if (isLocalPlayer) {
            Camera.main.GetComponent<FollowCamera>().target = this;
            originalAimPos = aimTarget.localPosition;
            aimSprite = aimTarget.GetComponentInChildren<SpriteRenderer>();

            MasterUI.instance.SetPlayer(this);

            // TODO: put in the game master script
            Cursor.visible = false;
        }
        else {
            aimTarget.gameObject.SetActive(false);
        }
    }

    [Mirror.Server]
    public void Damage(float dmg) {
        health -= dmg;
        if (health <= 0) {
            health = 0;
            dead = true;
            //TODO: play some sounds, instantiate some particles, create a ragdoll/dead body object
            Destroy(this.gameObject);
            }
    }

    [Mirror.Command]
    void CmdDie() {
        dead = true;
        health = 0;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update() {

        if (dead) return;

        if (isLocalPlayer || isServer) {
            energy -= energyDecayRate * Time.deltaTime;
            if (energy <= 0) {
                if (isLocalPlayer) {
                    CmdDie();
                } else {
                    Damage(health);
                }
            }
        }

        // actions (like shooting, charging, etc)
        if (isLocalPlayer) {

           

            // process input for actions
            Aim();

            //shooting
            if (Input.GetMouseButtonDown(0)) {
                CmdFire();
            }

        }
        
        //play everywhere
        Animation();

    }

    private void Aim() {
        // use vertical mouse axis to aim up and down
        verticalAim += Input.GetAxis("Mouse Y")*Time.deltaTime*verticalSensitivity;
        verticalAim = Mathf.Clamp(verticalAim, minAim, maxAim);


        //reset aim target position and color
        aimTarget.localPosition = originalAimPos;
        aimTarget.localRotation = Quaternion.identity;
        aimSprite.color = Color.white;

        //raycast out and look for a hit
        RaycastHit hitInfo;
        Vector3 origin = transform.position;
        origin.y = aimTarget.position.y;

        aimTarget.position = aimTarget.position + new Vector3(0, verticalAim, 0);

        Vector3 sightline = aimTarget.position - origin;
        
        //check for a hit
        if (Physics.Raycast(origin, sightline, out hitInfo, maxRange)) {
            sightline.Normalize();
            sightline *= (hitInfo.distance - 0.01f);
            //stick to the wall
            aimTarget.LookAt(aimTarget.transform.position + hitInfo.normal);
            //change color if it's an enemy
            if (hitInfo.collider.gameObject.GetComponent<Robot>() != null) {
                aimSprite.color = Color.red;
            }
        } else {
            sightline.Normalize();
            sightline *= maxRange;
        }

        aimTarget.position = origin + sightline;
        CmdAim(origin + sightline);
    }

    [Mirror.Command]
    private void CmdAim(Vector3 target) {
        aimTarget.position = target;
    }
    

    private void FixedUpdate() {
        // movement for local player (and other physics)
        if (!isLocalPlayer) {
            return;
        }

        controller.HandleMovement();
    }
    
    void Animation() {
        animator.SetFloat("Speed", rigidBody.velocity.magnitude);
    }

    [Mirror.Command]
    public void CmdFire() {
        GameObject projectile = Instantiate(weapon.projectilePrefab, weapon.projectileMount.position, transform.rotation);
        projectile.transform.LookAt(weapon.targetReticle);
        Mirror.NetworkServer.Spawn(projectile);
        RpcOnFire();
    }

    // animation update
    [Mirror.ClientRpc]
    void RpcOnFire() {
        animator.SetTrigger("Shoot");
    }

    public float PercentHealth() {
        return health / GetComponent<Health>().maxHealth;
    }

    public float PercentEnergy() {
        return energy / GetComponent<Energy>().maxEnergy;
    }

}
