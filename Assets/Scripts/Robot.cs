using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Controller))]
public class Robot : Mirror.NetworkBehaviour {

    [Header("Components")]
    private Animator animator;
    private Rigidbody rigidBody;

    [Header("Movement")]
    private Controller controller; //controller (AI or human)

    //    [Header("Firing")]
    //    public KeyCode shootKey = KeyCode.Space;
    //    public GameObject projectilePrefab;
    //    public Transform projectileMount;


    private void Awake() {
        controller = GetComponent<Controller>();

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

    }

    private void Start() {
        if (isLocalPlayer) {
            Camera.main.GetComponent<FollowCamera>().target = this.transform;
        }
    }

    // Update is called once per frame
    void Update() {

        // actions (like shooting, charging, etc)
        if (isLocalPlayer) {
            // process input for actions
        }

        UpdateAnimator();
    }

    private void FixedUpdate() {
        // movement for local player (and other physics)
        if (!isLocalPlayer) {
            return;
        }

        controller.HandleMovement();
    }


    //updates the animator with current robot state
    private void UpdateAnimator() {
        animator.SetFloat("Speed", rigidBody.velocity.magnitude);
    }
}
