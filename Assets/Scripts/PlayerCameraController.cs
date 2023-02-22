using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCameraController : MonoBehaviour {
    GameObject player;
    Transform[] cameraLocations;
    Vector3 initialOrbitAngles;
    Vector3 orbitAngles;
    Car playerInfo;
    int activeLocation = 1;

    public float speed = 20f;
    public float verticalRotationLimit = 45f;
    public float recenterSmooth = 0.9f;
    public float rotationSpeed = 1.1f;
    public float collisionAdjust = 0.3f;

    bool reverse = false;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInfo = player.GetComponent<Car>();
        cameraLocations = player.transform.Find("Camera Locations").GetComponentsInChildren<Transform>();
        initialOrbitAngles = cameraLocations[0].localRotation.eulerAngles;
    }

    CameraLocationMeta GetCurrentMeta() {
        return cameraLocations[activeLocation].gameObject.GetComponent<CameraLocationMeta>();
    }
    
    void DetectCollisionAndCulling() {
        CameraLocationMeta meta = GetCurrentMeta();
        if (meta.lockConstraints) {
            return;
        }

        RaycastHit hit;
        if (Physics.Linecast(cameraLocations[0].position, transform.position, out hit)) {
            transform.position = hit.point + transform.forward * collisionAdjust;
        }
    }

    void SwitchLocation() {
        if (activeLocation >= cameraLocations.Length - 1) {
            activeLocation = 1;
        } else {
            activeLocation++;
        }
    }

    void RecenterLook() {
        if (orbitAngles.y > recenterSmooth) {
            orbitAngles.y -= recenterSmooth;
        } else if (orbitAngles.y < -recenterSmooth) {
            orbitAngles.y += recenterSmooth;
        } else if (orbitAngles.y == recenterSmooth || orbitAngles.y == -recenterSmooth) {
            orbitAngles.y = 0;
        }

        if (orbitAngles.z > recenterSmooth) {
            orbitAngles.z -= recenterSmooth;
        } else if (orbitAngles.z < -recenterSmooth) {
            orbitAngles.z += recenterSmooth;
        } else if (orbitAngles.z == recenterSmooth || orbitAngles.z == -recenterSmooth) {
            orbitAngles.z = 0;
        }
    }

    void CalculateLook() {
        if (reverse) {
            return;
        }

        CameraLocationMeta meta = GetCurrentMeta();
        if (meta.lockConstraints) {
            orbitAngles = new Vector3();
            return;
        }

        bool isDriving = InputManager.instance.rightTrigger != 0;
        float mouseX = InputManager.instance.mouseX * rotationSpeed;
        float mouseY = InputManager.instance.mouseY * rotationSpeed;

        orbitAngles.y += isDriving ? mouseX * recenterSmooth * 1.35f : mouseX;
        orbitAngles.z -= isDriving ? mouseY * recenterSmooth * 1.35f : mouseY;

        orbitAngles.y = orbitAngles.y % 360;
        orbitAngles.z = Mathf.Clamp(orbitAngles.z, -verticalRotationLimit, verticalRotationLimit);
    }

    void Look() {
        cameraLocations[0].localRotation = Quaternion.Euler(initialOrbitAngles + orbitAngles);
    }

    void Follow() {
        Transform currentLocation = cameraLocations[activeLocation];
        CameraLocationMeta meta = GetCurrentMeta();

        float relativeSpeed = speed;
        if (meta.lockConstraints || reverse) {
            relativeSpeed = 100f;
        }

        Vector3 relativePosition = currentLocation.position;
        if (reverse) {
            relativePosition = cameraLocations[1].position;
        }
        transform.position = Vector3.Lerp(transform.position, relativePosition, Time.deltaTime * relativeSpeed);

        if (meta.lockConstraints && !reverse) {
            transform.rotation = player.transform.rotation;
        } else {
            transform.LookAt(cameraLocations[0].position);
        }
    }

    void Update() {
        Follow();
        CalculateLook();
        Look();
        DetectCollisionAndCulling();

        if (InputManager.instance.rightTrigger != 0 && !reverse) {
            RecenterLook();
        }

        if (InputManager.instance.fire3Down) {
            SwitchLocation();
        }
        if (InputManager.instance.fire4Down) {
            orbitAngles.y = 180;
            reverse = true;
        }
        if (InputManager.instance.fire4Up) {
            orbitAngles.y = 0;
            reverse = false;
        }
    }
}
