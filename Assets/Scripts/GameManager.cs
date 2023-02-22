using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public bool isPaused = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    public void SetIsPaused(bool paused) {
        isPaused = paused;
    }

    public void LockCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
