using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSelect : MonoBehaviour {

    public int maxChars = 15;

    public Text typedText;

    public void UpdateField() {
        if (typedText.text.Length > maxChars) {
            typedText.text = typedText.text.Substring(0, maxChars);
        }
    }

    public void FinishField() {
        string name = typedText.text;
        if (name.Length == 0) {
            name = "Surge-Bot";
        }

        LocalPlayerData.instance.PlayerName = name;
    }
}
