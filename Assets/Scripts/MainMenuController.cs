using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    Animator fadeAnimator;
    AudioSource select;

    bool selected = false;

    void Awake() {
        fadeAnimator = GameObject.Find("Fade").GetComponent<Animator>();
        select = GameObject.Find("Select SFX").GetComponent<AudioSource>();
    }

    void RedirectToCarSelectScreen() {
        SceneManager.LoadScene("Beltane");
    }

    void StartGame() {
        selected = true;
        fadeAnimator.Play("Fade Out");
        select.Play();
        Invoke("RedirectToCarSelectScreen", 1f);
    }

    void Update() {
        if (selected) {
            return;
        }
        if (InputManager.instance.pauseDown) {
            StartGame();
        }
    }
}
