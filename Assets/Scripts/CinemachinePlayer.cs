using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePlayer : MonoBehaviour {
    CinemachineFreeLook cm;
    Car car;

    [SerializeField]
    float minFov = 55f;

    [SerializeField]
    float maxFov = 65f;

    float initialFov;

    void Start() {
        car = GameObject.Find("Gamma").GetComponent<Car>();
        cm = GetComponent<CinemachineFreeLook>();
        initialFov = cm.m_Lens.FieldOfView;
    }

    void UpdateFov() {
        int velocity = Mathf.Abs(car.GetForwardVelocity());
        float currentFov = initialFov + (velocity / 5f);

        if (currentFov > maxFov) {
            currentFov = maxFov;
        } else if (currentFov < minFov) {
            currentFov = minFov;
        }

        cm.m_Lens.FieldOfView = currentFov;
    }

    void Update() {
        UpdateFov();

        if (InputManager.instance.IsAccelerating()) {
            cm.m_RecenterToTargetHeading.m_enabled = true;
        } else {
            cm.m_RecenterToTargetHeading.m_enabled = false;
        }

        if (Input.GetButton("Fire4")) {
            cm.m_RecenterToTargetHeading.m_enabled = false;
            cm.m_XAxis.Value = 180f;
        }
        if (Input.GetButtonUp("Fire4")) {
            cm.m_XAxis.Value = 0f;
        }
    }
}
