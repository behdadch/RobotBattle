using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovement : Controller {

    public float moveSpeed = 5;
    public float rotationSpeed = 200;
    public float jumpSpeed = 1;

    private Rigidbody rigidBody;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void HandleMovement() {

        /***** ROTATION/AIMING *****/
        float mouseRot = Input.GetAxis("Mouse X");

        mouseRot *= Time.fixedDeltaTime * rotationSpeed;

        transform.Rotate(transform.up, mouseRot);


        /***** LINEAR MOVEMENT *****/
        // move
        Vector3 motion = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        //if we have input then set our new velocity
        if (motion.sqrMagnitude > 0.01f) {
            motion.Normalize();
            motion = motion * moveSpeed;

            //moving backwards or strafing is slower
            motion.x = motion.x * 0.5f;
            if (motion.z < 0) {
                motion = motion * 0.8f;
            }

        } else {
            motion = Vector3.zero;
        }

        //handle jumping separately
        motion.y = 0;

        //keep falling if we already are
        float vertSpeed = rigidBody.velocity.y;


        //check for jumping
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f) && vertSpeed < 0.1f && Input.GetButton("Jump")) {
            vertSpeed = jumpSpeed;
        }

        if (motion.sqrMagnitude > 0.01f) {
            rigidBody.velocity = transform.rotation * motion + new Vector3(0, vertSpeed, 0);
        }
        else {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, vertSpeed, rigidBody.velocity.z);
        }

    }

}
