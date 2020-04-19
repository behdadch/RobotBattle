using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    public int rotationSpeedObject;
    void Update()
    {
        transform.Rotate(0,rotationSpeedObject,0,Space.World);
    }
}
