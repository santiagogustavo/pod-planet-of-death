using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {
    [SerializeField]

    GameObject canvas;
    bool isPaused;

    void Start() {
        canvas.SetActive(false);
    }

    void HandleVisibility () {
        if (isPaused) {
            canvas.SetActive(true);
            Time.timeScale = 0f;
        } else if (!isPaused) {
            canvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void Update() {
        if (Input.GetButtonDown("Pause")) {
            isPaused = !isPaused;
            HandleVisibility();
        }
    }
}
