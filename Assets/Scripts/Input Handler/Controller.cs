using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : Mirror.NetworkBehaviour {

    public abstract void HandleMovement();

    public abstract void Aim();

    public Transform aimTarget;

    [Mirror.Command]
    protected void CmdAim(Vector3 target) => SrvAim(target);

    [Mirror.Server]
    protected void SrvAim(Vector3 target) {
        aimTarget.position = target;
    }
}
