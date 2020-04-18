using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Mirror.NetworkBehaviour {

    [Header("Firing")]
    public KeyCode shootKey = KeyCode.Space;
    public GameObject projectilePrefab;
    public Transform projectileMount;
    public Transform targetReticle;

    
}
