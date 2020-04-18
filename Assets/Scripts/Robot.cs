using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Robot : Mirror.NetworkBehaviour {

    [Header("Components")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5;
    public float rotationSpeed = 100;

//    [Header("Firing")]
//    public KeyCode shootKey = KeyCode.Space;
//    public GameObject projectilePrefab;
//    public Transform projectileMount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        // movement for local player
        if (!isLocalPlayer)
            return;

        // rotate
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        // move
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
        animator.SetBool("Moving", agent.velocity != Vector3.zero);

        // actions
        
    }
}
