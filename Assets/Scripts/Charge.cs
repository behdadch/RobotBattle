using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{

    bool charging;
    private Robot robot;

    Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
        charging = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && charging == false)
        {
            robot = col.GetComponent<Robot>();
            charging = true; //to only let one robot charges at a time
            LightControl("InUse");
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<Robot>() == robot)
        {
            charging = false;
            LightControl("Exit");
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
    void LightControl(string status){
        animator.SetTrigger(status);
    }
    public void Broken(){
        animator.SetBool("isBroken", true); 
    //TODO
}
}