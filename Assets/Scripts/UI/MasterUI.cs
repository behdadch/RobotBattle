using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MasterUI : MonoBehaviour {

    public static MasterUI instance;

    public SliderPanelUI sliders;

    public Button exitButton;

    public Text topText;

    Robot player;

    bool hidden = false;

    List<GameObject> playersLeft;

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
        if (playersLeft != null) {
            for (int i = playersLeft.Count - 1; i >= 0; i--) {
                if (playersLeft[i] == null) {
                    playersLeft.RemoveAt(i);
                }
            }
            if (playersLeft.Count > 1) {
                topText.text = playersLeft.Count + " REMAIN!";
            }
            else {
                if (player == null) {
                    topText.text = "LIFE TERMINATED";
                } else {
                    topText.text = "LIFE MAINTAINED";
                }
            }
        } else {
            topText.text = "Waiting to begin...";
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
        Cursor.lockState = CursorLockMode.Confined;
        exitButton.gameObject.SetActive(false);
    }

    public void SetPlayer(Robot player) {
        this.player = player;
        sliders.player = player;
        playersLeft = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void Exit() {
        if (player != null) {
            player.Disconnect();
        } else {
            SceneManager.LoadScene(0);
        }
    }
}
