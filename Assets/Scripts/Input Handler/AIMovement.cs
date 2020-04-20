using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : Controller {

    public float followDistance = 5;
    public float aimError = 0.5f;
    public bool  scaleError = true;
    public float aimDotLimit = 0.5f; //cos(fov/2)


    public float moveSpeed = 5;
    public float rotationSpeed = 200;
    public float jumpSpeed = 1;

    private Rigidbody rigidBody;
    private NavMeshAgent agent;

    private GameObject target = null;

    public Transform firePoint;

    List<GameObject> players;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
    }

    public override void HandleMovement() {
        if (players == null) {
            //get all players
            players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            //return if none
            if (players == null) {
                return;
            }
            //remove self from list
            for (int i = players.Count - 1; i >= 0; i--) {
                if (players[i] == this.gameObject) {
                    players.RemoveAt(i);
                    break;
                }
            }
        }

        if (target == null) {
            //remove null players
            for (int i = players.Count - 1; i >= 0; i--) {
                if (players[i] == null) {
                    players.RemoveAt(i);
                    break;
                }
            }
            if (players.Count == 0) {
                return;
            }
            SetTarget();
            if (target == null) return;
        }

        float dist = (target.transform.position - transform.position).magnitude;

        RaycastHit hitData;
        bool canSee = false;
        Vector3 aimVector = (target.transform.position + new Vector3(0, 0.5f, 0) ) - firePoint.position;

        if (Physics.Raycast(firePoint.position, aimVector, out hitData) && Vector3.Dot(aimVector.normalized, transform.forward) < aimDotLimit) {
            canSee = hitData.collider.gameObject == target;
        }

        //if we can't see or are too far move to the enemy
        if (dist > followDistance && !canSee) {
            agent.SetDestination(target.transform.position);
        }
        else { //stand and shoot!
            Vector3 los = target.transform.position - transform.position;

            agent.SetDestination(transform.position + los.normalized * 0.1f);
        }
    }


    private void SetTarget() {
        float minDist = Mathf.Infinity;
        foreach (GameObject go in players) {
            if (go == null) {
                continue;
            }
            float dist = (transform.position - go.transform.position).magnitude;
            if (dist < minDist) {
                target = go;
            }
        }
    }


    public override void Aim() {
        if (target == null) {
            return;
        }

        //TODO implement aiming
        RaycastHit hitData;

        Vector3 aimVector = (target.transform.position + new Vector3(0, 0.5f, 0)) - firePoint.position;

        //don't aim if outside our FOV
        if (Vector3.Dot(aimVector.normalized, transform.forward) < aimDotLimit) {
            return;
        }

        Vector3 error = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * aimError;

        if (scaleError) {
            float dist = aimVector.magnitude;
            error = error * Mathf.Sqrt(Mathf.Min(dist-1, 0));
        }

        //raycast toward the target from the head (roughly)
        if (Physics.Raycast(firePoint.position, aimVector, out hitData)) {
            if (hitData.collider.gameObject == target) {
                SrvAim(hitData.point + error);
            }
        }

    }

}
