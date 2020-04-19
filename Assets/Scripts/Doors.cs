using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    Animator animator;
    bool doorOpen;
    int count;
    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
        count = 0; //No body has entered the door yet

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorOpen = true;
            DoorControl("Open");
            count++;
        }
    }
    void OnTriggerExit(Collider col){
        
        Debug.Log(count);
        if (col.gameObject.tag == "Player"){
            count--;
        }

        if(doorOpen && count==0){
            doorOpen = false;
            DoorControl("Close");
        }
    }
    void DoorControl(string status)
    {
        animator.SetTrigger(status);
    }
}
