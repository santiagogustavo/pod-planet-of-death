using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public float leftTrigger;
    public float rightTrigger;

    public bool fire1;
    public bool fire2;
    public bool fire3;
    public bool fire4;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Update() {
        leftTrigger = Input.GetAxis("Left Trigger");
        rightTrigger = Input.GetAxis("Right Trigger");
        fire1 = Input.GetButton("Fire1");
        fire2 = Input.GetButton("Fire2");
        fire3 = Input.GetButton("Fire3");
        fire4 = Input.GetButton("Fire4");
    }

    public bool IsBraking() {
        return leftTrigger > 0f;
    }

    public bool IsAccelerating() {
        return rightTrigger > 0f;
    }
}
