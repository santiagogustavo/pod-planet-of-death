using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionController : MonoBehaviour {
    [SerializeField]
    float debrisMinVelocity = 50f;

    GameObject debris;
    GameObject sparks;
    Car car;

    void Start() {
        car = GetComponent<Car>();
        debris = (GameObject)Resources.Load("Particles/Debris");
        sparks = (GameObject) Resources.Load("Particles/Sparks");
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude < debrisMinVelocity || car.GetForwardVelocity() == 0) {
            return;
        }
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.zero, contactPoint);

        Instantiate(debris, contactPoint, rotation, transform);
    }

    void OnCollisionStay(Collision collision) {
        if (car.GetForwardVelocity() == 0) {
            return;
        }
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);

        Instantiate(sparks, contactPoint, rotation, transform);
    }
}
