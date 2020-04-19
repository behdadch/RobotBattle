using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocalPlayerData : MonoBehaviour {

    public string PlayerName;

    public static LocalPlayerData instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(instance.gameObject); 
        }
    }

    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }
 

}
