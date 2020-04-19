using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHostUI : MonoBehaviour
{
    public void StartButton() {
        LobbyManager.instance.StartButton();
    }

    public void CancelButton() {
        LobbyManager.instance.CancelButton();
    }
}
