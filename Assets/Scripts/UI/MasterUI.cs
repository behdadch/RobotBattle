using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterUI : MonoBehaviour {

    public static MasterUI instance;

    public SliderPanelUI sliders;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }

    public void SetPlayer(Robot player) {
        sliders.player = player;
    }
}
