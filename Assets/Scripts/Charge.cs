using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{

    bool charging = false;
    private Robot robot;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && charging == false)
        {
            robot = col.GetComponent<Robot>();
            charging = true;
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<Robot>() == robot)
        {
            charging = false;
        }
    }
    void Update()
    {
        if (charging == true && robot!= null)
        {
            float extraEnergy =  5 * Time.deltaTime;
             Debug.Log(extraEnergy);
            robot.AddEnergy(extraEnergy);
        }
    }
    void Broken(){
    //TODO
}
}