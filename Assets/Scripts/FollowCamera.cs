using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {


    public Transform target;
    public Vector3 boom = new Vector3(0.2f, 0.8f, -1.4f);
    public Vector3 lead = new Vector3(0, 0, 5);



    // LateUpdate is called once per frame after Update
    void LateUpdate() {
        if (target == null) {
            FPSControls();
        }
        else {
            FollowTarget();
        }        
    }


    private void FollowTarget() {
        transform.position = target.position + target.rotation * boom;
        transform.LookAt(target.position + target.rotation * lead);
    }

    private void FPSControls() {

    }

}
