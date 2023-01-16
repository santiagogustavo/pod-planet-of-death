using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public enum DriveMode {
        FWD,
        RWD,
        AWD
    };

    public GameObject[] wheels;
    public GameObject[] brakeLights;
    public GameObject[] rearLights;
    public ParticleSystem[] tireSmokes;
    public Material brakeMaterial;
    public Texture brakeTexture;

    public DriveMode driveMode = DriveMode.RWD;
    public float maxSpeed = 100f;
    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 45f;
    public float turnSpeed = 2f;
    public float stabilizationSpeed = 1f;
    public GameObject centerOfMass;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    Rigidbody rb;
    Texture originalEmissionMap;

    void Start() {
        originalEmissionMap = brakeMaterial.GetTexture("_EmissionMap");
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;

        SetBrakeLights(false);
        SetRearLights(false);
    }

    Vector3 GetLocalVelocity() {
        return transform.InverseTransformDirection(rb.velocity);
    }

    public int GetForwardVelocity() {
        return Mathf.RoundToInt(GetLocalVelocity().z);
    }

    bool IsStopped() {
        return Mathf.RoundToInt(GetLocalVelocity().z) < 3f;
    }

    void SetBrakeLights(bool on) {
        if (on) {
            brakeMaterial.SetTexture("_EmissionMap", brakeTexture);
        } else {
            brakeMaterial.SetTexture("_EmissionMap", originalEmissionMap);
        }
        foreach (GameObject brakeLight in brakeLights) {
            brakeLight.SetActive(on);
        }
    }

    void SetRearLights(bool on) {
        if (on) {
            brakeMaterial.SetTexture("_EmissionMap", brakeTexture);
        } else {
            brakeMaterial.SetTexture("_EmissionMap", originalEmissionMap);
        }
        foreach (GameObject rearLight in rearLights) {
            rearLight.SetActive(on);
        }
    }

    void PlayTireSmokesIfGrounded() {
        for (int i = 0; i < wheels.Length; i++) {
            if (wheels[i].GetComponent<WheelCollider>().isGrounded) {
                tireSmokes[i].Play();
            }
        }
    }

    bool IsFwd() {
        return driveMode == DriveMode.FWD || driveMode == DriveMode.AWD;
    }

    bool IsRwd() {
        return driveMode == DriveMode.RWD || driveMode == DriveMode.AWD;
    }

    void RotateWheelModels() {
        for (int i = 0; i < wheels.Length; i++) {
            WheelCollider wheelCollider = wheels[i].GetComponent<WheelCollider>();
            Transform wheelTransform = wheels[i].GetComponent<Transform>();

            /* Steering rotation */
            if (wheels[i].name == "FL" || wheels[i].name == "FR") {
                wheelTransform.localEulerAngles = new Vector3(
                    wheelTransform.localEulerAngles.x,
                    wheelCollider.steerAngle,
                    wheelTransform.localEulerAngles.z
                );
            }

            /* Speed rotation */
            wheelTransform.eulerAngles = new Vector3(
                wheelTransform.eulerAngles.x,
                wheelTransform.eulerAngles.y,
                wheelTransform.eulerAngles.z + GetLocalVelocity().z * 35f * Time.deltaTime
            );
        }
    }

    void ApplyTorqueToWheels() {
        for (int i = 0; i < wheels.Length; i++) {
            WheelCollider wheelCollider = wheels[i].GetComponent<WheelCollider>();

            /* Front wheels steering */
            if (wheels[i].name == "FL" || wheels[i].name == "FR") {
                wheelCollider.steerAngle = currentTurnAngle;
            }

            /* Torque wheels */
            if (IsFwd() && (wheels[i].name == "FL" || wheels[i].name == "FR")) {
                wheelCollider.motorTorque = currentAcceleration;
            }
            if (IsRwd() && (wheels[i].name == "BL" || wheels[i].name == "BR")) {
                wheelCollider.motorTorque = currentAcceleration;
            }

            wheelCollider.brakeTorque = currentBrakeForce;
        }
    }

    void ComputeTurnAngle() {
        float horizontal = Input.GetAxis("Horizontal");

        if (currentTurnAngle > 0f) {
            currentTurnAngle -= stabilizationSpeed;
        } else if (currentTurnAngle < 0f) {
            currentTurnAngle += stabilizationSpeed;
        }

        currentTurnAngle += horizontal * turnSpeed;
        currentTurnAngle = Mathf.Clamp(currentTurnAngle, -maxTurnAngle, maxTurnAngle);
    }

    void ComputeAcceleration() {
        if (GetForwardVelocity() > maxSpeed) {
            currentAcceleration = 0f;
            return;
        }

        bool reverse = IsStopped() && Input.GetAxis("Vertical") == -1f;

        if (reverse) {
            currentAcceleration = -(acceleration + rb.mass);
        } else {
            currentAcceleration = 0f;
        }

        if (InputManager.instance.IsAccelerating()) {
            currentAcceleration = (acceleration + rb.mass) * InputManager.instance.rightTrigger;
        } else if (!reverse) {
            currentAcceleration = 0f;
        }
    }

    void ComputeBraking() {
        if (InputManager.instance.IsBraking()) {
            currentBrakeForce = (brakingForce + rb.mass) * InputManager.instance.leftTrigger;
        } else {
            currentBrakeForce = 0f;
        }
    }

    void ComputeInputs() {
        ComputeTurnAngle();
        ComputeAcceleration();
        ComputeBraking();
    }

    void FixedUpdate() {
        ApplyTorqueToWheels();
    }

    void Update() {
        ComputeInputs();
        bool reverse = IsStopped() && Input.GetAxis("Vertical") == -1f;

        if (InputManager.instance.IsAccelerating()) {
            if (Mathf.Max(0, Mathf.RoundToInt(GetLocalVelocity().z)) < 3f) {
                PlayTireSmokesIfGrounded();
            }
        }

        if (reverse) {
            SetRearLights(true);
        } else {
            SetRearLights(false);
        }

        if (InputManager.instance.IsBraking()) {
            SetBrakeLights(true);
        } else if (!reverse) {
            SetBrakeLights(false);
        }

        RotateWheelModels();
    }
}
