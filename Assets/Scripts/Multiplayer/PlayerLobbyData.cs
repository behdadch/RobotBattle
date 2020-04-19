using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLobbyData : Mirror.NetworkBehaviour {

    [Mirror.SyncVar]
    public string playerName;

    Mirror.NetworkManager manager;


    public GameObject robot;

    // Start is called before the first frame update
    void Start() {
        manager = Mirror.NetworkManager.singleton;
        if (isLocalPlayer) {
            CmdNameUpdate(LocalPlayerData.instance.PlayerName);

            //show the Start button and link it to myself
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("ServerButton");
            buttons[buttons.Length - 1].GetComponent<Button>().onClick.AddListener(CmdCancel);
            if (isServer) {
                buttons[0].GetComponent<Button>().onClick.AddListener(CmdStartButton);
            } //NOTE: something else destroys the play button if it's not the host :-)

        }
    }


    [Mirror.Command]
    public void CmdNameUpdate(string name) {
        playerName = name;
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



    [Mirror.Command]
    public void CmdCancel() {
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
