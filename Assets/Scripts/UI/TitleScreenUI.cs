using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour {

    public RectTransform mainButtons;
    public RectTransform optionsMenu;
    public RectTransform credits;

    RectTransform currentScreen;

    private void Start() {
        currentScreen = mainButtons;
    }

    public void SinglePlayer() {

    }

    public void Multiplayer() {

    }

    public void Options() {
        StartCoroutine("TweenIn", optionsMenu);
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = optionsMenu;
    }

    public void Credits() {
        StartCoroutine("TweenIn", credits);
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = credits;
    }

    public void MainMenu() {
        StartCoroutine("TweenIn", mainButtons);
        StartCoroutine("TweenOut", currentScreen);
        currentScreen = mainButtons;
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
