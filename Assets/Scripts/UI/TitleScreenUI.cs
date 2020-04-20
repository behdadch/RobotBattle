using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour {

    public RectTransform mainButtons;
    public RectTransform multiplayer;
    public RectTransform optionsMenu;
    public RectTransform credits;

    public Text ipField;

    public GameObject robotPrefab;

    RectTransform currentScreen;

    private void Start() {
        currentScreen = mainButtons;
    }

    public void SinglePlayer() {
        Mirror.NetworkManager.singleton.StartHost();
        Mirror.NetworkManager.singleton.onlineScene = "LoganScene";
        Mirror.NetworkManager.singleton.autoCreatePlayer = true;
        Mirror.NetworkManager.singleton.playerPrefab = robotPrefab;
        Mirror.NetworkManager.singleton.StartHost();
        //SceneManager.LoadScene("LoganScene");
    }

    public void Multiplayer() {
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = multiplayer;
        StartCoroutine("TweenIn", currentScreen);
    }

    public void HostGame() {
        Mirror.NetworkManager.singleton.StartHost();
    }

    public void JoinGame() {
        string ip = ipField.text;
        if (ip.Length == 0) {
            ip = "localhost";
        }

        Mirror.NetworkManager.singleton.networkAddress = ip;
        Mirror.NetworkManager.singleton.StartClient();

    }

    public void Options() {
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = optionsMenu;
        StartCoroutine("TweenIn", currentScreen);
    }

    public void Credits() {
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = credits;
        StartCoroutine("TweenIn", currentScreen);
    }

    public void MainMenu() {
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = mainButtons;
        StartCoroutine("TweenIn", currentScreen);
    }

    IEnumerator TweenIn(RectTransform target) {
        for (float ft = 0; ft <= 1; ft += Time.deltaTime) {
            target.localPosition = Vector3.Lerp(target.localPosition, Vector3.zero, ft);
            yield return null;
        }
    }

    IEnumerator TweenOut(RectTransform target) {
        for (float ft = 0; ft <= 1; ft += Time.deltaTime) {
            target.localPosition = Vector3.Lerp(target.localPosition, new Vector3(0, -5000, 0), ft);
            yield return null;
        }
    }

    public void Exit() {
        Application.Quit(0);
    }
}
