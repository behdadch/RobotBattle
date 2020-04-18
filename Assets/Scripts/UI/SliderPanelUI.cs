using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPanelUI : MonoBehaviour {

    public Robot player;

    public Slider healthSlider;
    public Slider energySlider;

    // Update is called once per frame
    void Update() {
        if (player == null) {
            return;
        }

        healthSlider.value = player.PercentHealth();
        energySlider.value = player.PercentEnergy();
    }
}
