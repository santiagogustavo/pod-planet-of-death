using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISpeedController : MonoBehaviour {
    Car player;
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Car>();
    }

    void Update() {
        text.text = player.GetForwardVelocity().ToString();
    }
}
