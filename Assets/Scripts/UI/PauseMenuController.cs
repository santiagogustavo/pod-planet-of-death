using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    [SerializeField]
    GameObject canvas;

    bool isPaused = false;
    int activeButton = 0;
    float debounceTime = 0.5f;
    float inputTime = 0f;

    GameObject[] buttons;

    void Awake() {
        inputTime = debounceTime;

        buttons = GameObject.FindGameObjectsWithTag("Menu Button");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(buttons, myComparer);
    }

    void PauseGame() {
        isPaused = true;
        GameManager.instance.SetIsPaused(isPaused);
        activeButton = 0;
        Time.timeScale = 0f;
        UpdateButtons();
    }

    void ResumeGame() {
        isPaused = false;
        GameManager.instance.SetIsPaused(isPaused);
        activeButton = 0;
        Time.timeScale = 1f;
        UpdateButtons();
    }

    void RedirectToMainMenu() {
        ResumeGame();
        SceneManager.LoadScene("Main Menu");
    }

    void UpdateButtons() {
        for (int i = 0; i < buttons.Length; i++) {
            if (i == activeButton) {
                buttons[i]?.GetComponent<MenuButtonController>()?.SetImageAsPressed();
            } else {
                buttons[i]?.GetComponent<MenuButtonController>()?.SetImageAsOriginal();
            }
        }
    }

    void Debounce() {
        inputTime = 0f;
    }

    bool Debounced() {
        return inputTime >= debounceTime;
    }

    void NextButton() {
        Debounce();
        if (activeButton == buttons.Length - 1) {
            activeButton = 0;
        } else {
            activeButton += 1;
        }
        UpdateButtons();
        Invoke("Debounce", 0.2f);
    }

    void PreviousButton() {
        Debounce();
        if (activeButton == 0) {
            activeButton =  buttons.Length - 1;
        } else {
            activeButton -= 1;
        }
        UpdateButtons();
    }

    void SelectCurrent() {
        switch (activeButton) {
            case 0: {
                isPaused = false;
                ResumeGame();
                break;
            }
            case 1: {
                RedirectToMainMenu();
                break;
            }
            case 2: {
                Application.Quit();
                break;
            }
            default:
                break;
        }
    }

    void Update() {
        inputTime += Time.fixedDeltaTime;

        if (isPaused) {
            GameManager.instance.UnlockCursor();
            canvas.SetActive(true);
        } else {
            GameManager.instance.LockCursor();
            canvas.SetActive(false);
        }

        if (InputManager.instance.pauseDown && !isPaused) {
            PauseGame();
        } else if (InputManager.instance.pauseDown && isPaused) {
            ResumeGame();
        }

        if (InputManager.instance.up && Debounced()) {
            PreviousButton();
        }
        if (InputManager.instance.down && Debounced()) {
            NextButton();
        }
        if (InputManager.instance.fire1) {
            SelectCurrent();
        }
    }
}
