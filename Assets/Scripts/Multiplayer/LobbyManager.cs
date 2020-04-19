using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class LobbyManager : Mirror.NetworkBehaviour {

    Mirror.NetworkManager manager;
    public static LobbyManager instance;

    public GameObject robot;
    public GameObject playerData;


    private void Awake() {
        if (instance != null) {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
        manager = GetComponent<Mirror.NetworkManager>();
    }

    private void Start() {
        manager = Mirror.NetworkManager.singleton;

    }


    [Mirror.Command]
    public void CmdStartButton() {
        //command the server to start the game
        manager.playerPrefab = robot;
        manager.autoCreatePlayer = true;
        RpcClientStart();
        manager.ServerChangeScene("LoganScene");
        manager.onlineScene = "LoganScene";
    }


    [Mirror.ClientRpc]
    void RpcClientStart() {
        //set auto spawn to true
        //set player object to robot
        manager.playerPrefab = robot;
        manager.autoCreatePlayer = true;
    }


    public void CancelButton() {

        if (Mirror.NetworkServer.active && Mirror.NetworkClient.isConnected) {
            manager.StopHost();
        } else if (Mirror.NetworkClient.isConnected) {
            manager.StopClient();
        } else {
            //I'm not sure why this is happening
        }


        DestroyImmediate(manager.gameObject);
        SceneManager.LoadScene(0);

    }
}
