using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public GameObject[] wheels;
    public GameObject[] brakeLights;
    public ParticleSystem[] tireSmokes;
    public Material brakeMaterial;
    public Texture brakeTexture;

    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 45f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    public GameObject centerOfMass;
    Rigidbody rb;
    Texture originalEmissionMap;

    void Start() {
        originalEmissionMap = brakeMaterial.GetTexture("_EmissionMap");
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;

        SetBrakeLights(false);
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

    void PlayTireSmokesIfGrounded() {
        for (int i = 0; i < wheels.Length; i++) {
            if (wheels[i].GetComponent<WheelCollider>().isGrounded) {
                tireSmokes[i].Play();
            }
        }
    }

    void FixedUpdate() {
        if (Input.GetButton("Fire1")) {
            currentAcceleration = acceleration;
        } else {
            currentAcceleration = 0f;
        }

        if (Input.GetButton("Fire2")) {
            currentBrakeForce = brakingForce;
        } else {
            currentBrakeForce = 0f;
        }

        currentTurnAngle = maxTurnAngle * Input.GetAxisRaw("Horizontal");

        wheels[0].GetComponent<WheelCollider>().motorTorque = currentAcceleration;
        wheels[1].GetComponent<WheelCollider>().motorTorque = currentAcceleration;

        wheels[0].GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
        wheels[1].GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
        wheels[2].GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
        wheels[3].GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;

        for (int i = 0; i < wheels.Length; i++) {
            WheelCollider wheelCollider = wheels[i].GetComponent<WheelCollider>();
            if (i < 2) {
                wheelCollider.steerAngle = currentTurnAngle;
            }
        }
    }

    void Update() {
        if (Input.GetButton("Fire1")) {
            if (Mathf.Abs(rb.velocity.x) < 0.15f) {
                PlayTireSmokesIfGrounded();
            }
        }

        if (Input.GetButton("Fire2")) {
            SetBrakeLights(true);
        } else {
            SetBrakeLights(false);
        }

        for (int i = 0; i < wheels.Length; i++) {
            WheelCollider wheelCollider = wheels[i].GetComponent<WheelCollider>();
            Transform wheelTransform = wheels[i].GetComponent<Transform>();
            if (i < 2) {
                wheelTransform.localEulerAngles = new Vector3(
                    wheelTransform.localEulerAngles.x,
                    wheelCollider.steerAngle,
                    wheelTransform.localEulerAngles.z
                );
            }
            wheelTransform.eulerAngles = new Vector3(wheelTransform.eulerAngles.x, wheelTransform.eulerAngles.y, rb.velocity.x * acceleration * Time.deltaTime);
        }
    }
}
