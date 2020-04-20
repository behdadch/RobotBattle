using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Energy))]
public class Robot : Mirror.NetworkBehaviour
{

    [Mirror.SyncVar]
    private float health = 100;
    [Mirror.SyncVar]
    private float energy = 100;
    [Mirror.SyncVar]
    public float energyDecayRate = 0.5f;
    [Mirror.SyncVar]
    public bool inControl = true;
    
    public void AddHealth(float qty){
        health = Mathf.Max(qty + health,100);
    }

    [Mirror.SyncVar]
    public float weaponCooldown = 0.5f;

    [Mirror.SyncVar]
    private float cdTime = 0f;

    public bool isAI = false;

    public void AddEnergy(float qty)
    {
        energy = Mathf.Max(qty + energy, 100);
    }

    [Header("Components")]
    private Animator animator;
    private Rigidbody rigidBody;

    [Header("Movement")]
    private Controller controller; //controller (AI or human)

    //    [Header("Firing")]
    //    public KeyCode shootKey = KeyCode.Space;
    //    public GameObject projectilePrefab;
    //    public Transform projectileMount;

    
    //health and energy
    private bool dead = false;

    private Weapon weapon;


    private void Awake()
    {
        controller = GetComponent<Controller>();

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        weapon = GetComponent<Weapon>();
    }


    public void AllowControl(bool val) {
        CmdControl(val);
        Debug.Log("requesting control to be " + val);
    }

    [Mirror.Command]
    private void CmdControl(bool val) {
        inControl = val;
    }

    private void Start() {

        health = GetComponent<Health>().maxHealth;
        energy = GetComponent<Energy>().maxEnergy;
        if (isLocalPlayer && !isAI)
        {
            Camera.main.GetComponent<FollowCamera>().target = this;
            MasterUI.instance.SetPlayer(this);
        }
        else {
            weapon.targetReticle.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    [Mirror.Server]
    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
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
    void Update()
    {

        if (dead) return;

        if (isLocalPlayer && !isAI) {
            CmdUpdate();
        }
        if (isServer && isAI) {
            SrvUpdate();
        }

        // actions (like shooting, charging, etc)
        if (( (isLocalPlayer && !isAI) || (isServer && isAI)) && inControl) {
            // process input for actions
            controller.Aim();
            //shooting
            if (isLocalPlayer && Input.GetMouseButtonDown(0) && !isAI) {
                CmdFire();
            }
            else if (isServer && isAI) {
                Vector3 target = weapon.targetReticle.position;
                Collider[] others = Physics.OverlapSphere(target, 0.2f);
                bool seeEnemy = false;

                foreach (Collider c in others) {
                    if (c.CompareTag("Player")) {
                        seeEnemy = true; break;
                    }
                }

                if (weapon.targetReticle && seeEnemy) {
                    SrvFire();
                }
            }
        }

        //play everywhere
        Animation();

    }

    [Mirror.Command]
    private void CmdUpdate() => SrvUpdate();

    [Mirror.Server]
    private void SrvUpdate() {
        cdTime -= Time.deltaTime;
        energy -= energyDecayRate * Time.deltaTime;
        cdTime = Mathf.Max(cdTime, 0);
        if (energy <= 0) {
            if (isLocalPlayer) {
                CmdDie();
            } else {
                Damage(health);
            }
        }
    }


    private void FixedUpdate()
    {
        // movement for local player (and other physics)
        if ((isLocalPlayer && inControl) || (isServer && isAI && inControl)) {
            controller.HandleMovement();
        }

        
    }

    void Animation()
    {
        animator.SetFloat("Speed", rigidBody.velocity.magnitude);
    }

    [Mirror.Server]
    public void SrvFire() {
        if (cdTime > 0) {
            return;
        }
        GameObject projectile = Instantiate(weapon.projectilePrefab, weapon.projectileMount.position, transform.rotation);
        projectile.transform.LookAt(weapon.targetReticle);
        Mirror.NetworkServer.Spawn(projectile);
        cdTime = weaponCooldown;
        RpcOnFire();
    }

    [Mirror.Command]
    public void CmdFire() => SrvFire();

    // animation update
    [Mirror.ClientRpc]
    void RpcOnFire()
    {
        animator.SetTrigger("Shoot");
    }

    public float PercentHealth()
    {
        return health / GetComponent<Health>().maxHealth;
    }

    public float PercentEnergy()
    {
        return energy / GetComponent<Energy>().maxEnergy;
    }


    public void Disconnect() {
        Debug.Log("Disconnect hit!");
        if (Mirror.NetworkServer.active && Mirror.NetworkClient.isConnected) {
            Mirror.NetworkManager.singleton.StopHost();
        } else if (Mirror.NetworkClient.isConnected) {
            Mirror.NetworkManager.singleton.StopClient();
        } else {
            //I'm not sure why this is happening
        }
        DestroyImmediate(Mirror.NetworkManager.singleton.gameObject);
        SceneManager.LoadScene(0);
    }

}
