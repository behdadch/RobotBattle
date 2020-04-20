using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovement : Controller {

    public float moveSpeed = 5;
    public float rotationSpeed = 200;
    public float jumpSpeed = 1;


    [SerializeField] private float zoomSpeed = 20f;

    private Rigidbody rigidBody;


    private float verticalAim;
    
    private Vector3 originalAimPos;
    private SpriteRenderer aimSprite;
    [SerializeField] private float verticalSensitivity = 10;


    [Header("Stats")]
    [SerializeField] private float maxAim = 2f;
    [SerializeField] private float minAim = -2f;
    [SerializeField] private float maxRange = 100;


    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();

    }

    private void Start() {
        if (isLocalPlayer) {
            originalAimPos = aimTarget.localPosition;
            aimSprite = aimTarget.GetComponentInChildren<SpriteRenderer>();

        } else {
            aimTarget.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (isLocalPlayer) {
            float zoom = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            Camera.main.GetComponent<FollowCamera>().ZoomIn(zoom);
        }
    }

    public override void HandleMovement() {

        /***** ROTATION/AIMING *****/
        float mouseRot = Input.GetAxis("Mouse X");
        mouseRot *= Time.fixedDeltaTime * rotationSpeed;

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
        //the transform is at the character's feet, so only check 5cm below to see if we're on the ground
        if (Physics.Raycast(transform.position, Vector3.down, 0.05f) && vertSpeed < 0.1f && Input.GetButton("Jump")) {
            vertSpeed = jumpSpeed;
        }

        Vector3 velocity = new Vector3();

        if (motion.sqrMagnitude > 0.01f) {
            velocity = transform.rotation * motion + new Vector3(0, vertSpeed, 0);
        }
        else {
            velocity = new Vector3(rigidBody.velocity.x, vertSpeed, rigidBody.velocity.z);
        }

        //motion prediction
        rigidBody.velocity = velocity;
        transform.Rotate(transform.up, mouseRot);
        //send to server
        CmdPhysicsUpdate(velocity, mouseRot);

    }



    public override void Aim() {
        // use vertical mouse axis to aim up and down
        verticalAim += Input.GetAxis("Mouse Y") * Time.deltaTime * verticalSensitivity;
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
    private void CmdPhysicsUpdate(Vector3 velocity, float rotation) {
        rigidBody.velocity = velocity;
        transform.Rotate(transform.up, rotation);
        //tell the clients to update -- NOTE: this should be handled by the networked transform?
        RpcPhysicsUpdate(velocity, rotation);
    }
    
    [Mirror.ClientRpc]
    private void RpcPhysicsUpdate(Vector3 velocity, float rotation) {
        //update rigidbody info on clients
        if (!isLocalPlayer) {
            rigidBody.velocity = velocity;
        }
    }

}
