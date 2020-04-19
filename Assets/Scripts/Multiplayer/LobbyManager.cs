using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {

    Mirror.NetworkManager manager;
    public static LobbyManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
        manager = GetComponent<Mirror.NetworkManager>();
    }



    public void StartButton() {
        //set the scene manager to automatically spawn players and transition to the MP scene
        //manager.autoCreatePlayer = true;
        manager.ServerChangeScene("LoganScene");
        manager.onlineScene = "LoganScene";
    }


    public void CancelButton() {
        manager.StopHost();
        Destroy(manager.gameObject);
        SceneManager.LoadScene(0);
    }
}
