using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MasterUI : MonoBehaviour {

    public static MasterUI instance;

    public SliderPanelUI sliders;

    public Button exitButton;

    Robot player;

    bool hidden = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }

    private void Start() {
        Hide();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (hidden) {
                Show();
            } else {
                Hide();
            }
        }
    }


    private void Show() {
        if (player != null) {
            player.AllowControl(false);
        }
        hidden = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        exitButton.gameObject.SetActive(true);
    }

    private void Hide() {
        if (player != null) {
            player.AllowControl(true);
        }
        hidden = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        exitButton.gameObject.SetActive(false);
    }

    public void SetPlayer(Robot player) {
        this.player = player;
        sliders.player = player;
        exitButton.onClick.AddListener(player.Disconnect);
    }
}
