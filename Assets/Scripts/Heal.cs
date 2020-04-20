using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    // Start is called before the first frame update
    public int healPercentage;
    private Robot robot;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            robot = col.GetComponent<Robot>();
            robot.AddHealth(healPercentage);
            Destroy(this.gameObject);
        }
    }


}
