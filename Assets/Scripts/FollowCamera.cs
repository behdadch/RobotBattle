using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {


    public Robot target;
    public Vector3 boom = new Vector3(0.2f, 0.8f, -1.4f);
    public Vector3 lead = new Vector3(0, 0, 5);

    public float speed = 15f;

    public float timeConstant = 3;
    public float rotTimeConstant = 3;

    private float defaultLength;

    public float minLength = 1;
    public float maxLength = 5;

    private void Awake() {
        defaultLength = boom.magnitude;
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate() {
        if (target == null) {
            FPSControls();
        }
        else {
            FollowTarget();
        }


    }

    public void ZoomIn(float qty) {
        float newLength = boom.magnitude + qty;

        newLength = Mathf.Clamp(newLength, minLength, maxLength);

        boom = boom.normalized * newLength;
    }

    private void FollowTarget() {
        //update the camera position
        Vector3 targetPosition = target.transform.position + target.transform.rotation * boom;

        transform.position = Vector3.Lerp(transform.position, targetPosition, timeConstant * Time.deltaTime);

        //check if a wall is in the way
        Vector3 origin = target.transform.position;
        origin.y = transform.position.y;
        Vector3 view = origin - transform.position; //negative view (from source);

        RaycastHit hitData;
        if (Physics.Raycast(origin, view, out hitData, view.magnitude)) {
            if (hitData.collider.gameObject != target) {
         //       transform.position = hitData.point;
            }
            Debug.Log("Cam raycast hit!");
        }

        //track 
        Vector3 targetLook = target.transform.position + target.transform.forward * 5f;
        Vector3 currentLook = transform.rotation * Vector3.forward + transform.position;

        transform.LookAt(Vector3.Lerp(currentLook, targetLook, rotTimeConstant * Time.deltaTime));
    }

    private void FPSControls() {
        Vector3 motion = Input.GetAxisRaw("Horizontal") * transform.right 
                       - Input.GetAxisRaw("Vertical") * transform.forward
                       + Input.mouseScrollDelta.y * transform.up * 5f;
        
        transform.position += transform.rotation * motion.normalized * speed * Time.deltaTime; ;

    }

}
