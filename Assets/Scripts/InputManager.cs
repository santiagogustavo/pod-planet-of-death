using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public float horizontal;
    public float vertical;
    public float leftTrigger;
    public float rightTrigger;
    public float mouseX;
    public float mouseY;

    public bool up;
    public bool down;
    public bool fire1;
    public bool fire2;
    public bool fire3;
    public bool fire4;
    public bool pause;

    public bool fire1Down;
    public bool fire2Down;
    public bool fire3Down;
    public bool fire4Down;
    public bool pauseDown;

    public bool fire1Up;
    public bool fire2Up;
    public bool fire3Up;
    public bool fire4Up;
    public bool pauseUp;

    public float acceleration;
    public float braking;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        up = vertical > 0f;
        down = vertical < 0f;
        leftTrigger = Input.GetAxis("Left Trigger");
        rightTrigger = Input.GetAxis("Right Trigger");

        fire1 = Input.GetButton("Fire1");
        fire2 = Input.GetButton("Fire2");
        fire3 = Input.GetButton("Fire3");
        fire4 = Input.GetButton("Fire4");
        pause = Input.GetButton("Pause");

        fire1Down = Input.GetButtonDown("Fire1");
        fire2Down = Input.GetButtonDown("Fire2");
        fire3Down = Input.GetButtonDown("Fire3");
        fire4Down = Input.GetButtonDown("Fire4");
        pauseDown = Input.GetButtonDown("Pause");

        fire1Up = Input.GetButtonUp("Fire1");
        fire2Up = Input.GetButtonUp("Fire2");
        fire3Up = Input.GetButtonUp("Fire3");
        fire4Up = Input.GetButtonUp("Fire4");
        pauseUp = Input.GetButtonUp("Pause");

        //acceleration = rightTrigger + vertical;
        //braking = leftTrigger - vertical;
        acceleration = rightTrigger;
        braking = leftTrigger;
    }

    public bool IsBraking() {
        return braking > 0f;
    }

    public bool IsAccelerating() {
        return acceleration > 0f;
    }
}
