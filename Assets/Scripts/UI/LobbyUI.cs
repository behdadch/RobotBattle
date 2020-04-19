using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : Mirror.NetworkBehaviour {

    public GameObject startButton;


    public Transform playerHolder;
    public GameObject playerPanel;

    void Start() {
        //hide the start button if we're not the server
        if (!isServer) {
            startButton.SetActive(false);
        }

    }

    [Mirror.ServerCallback]
    void Update() {

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (Transform t in playerHolder) {
            Destroy(t.gameObject);
        }

        foreach (GameObject go in players) {

            GameObject newPanel = Instantiate(playerPanel, playerHolder);

            newPanel.GetComponentInChildren<Text>().text = go.GetComponent<PlayerLobbyData>().playerName;
        }

        RpcClients();

    }

    [Mirror.ClientRpc]
    void RpcClients() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (Transform t in playerHolder) {
            Destroy(t.gameObject);
        }

        foreach (GameObject go in players) {

            GameObject newPanel = Instantiate(playerPanel, playerHolder);

            newPanel.GetComponentInChildren<Text>().text = go.GetComponent<PlayerLobbyData>().playerName;
        }
    }

    [Mirror.Client]
    public void StartButton() {
        Debug.Log("Start button pressed " + isServer );
        LobbyManager.instance.CmdStartButton();
    }

    [Mirror.Client]
    public void CancelButton() {
        LobbyManager.instance.CancelButton();
    }
}
