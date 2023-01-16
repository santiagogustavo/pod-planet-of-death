using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionController : MonoBehaviour {
    GameObject sparks;

    void Start() {
        sparks = (GameObject) Resources.Load("Particles/Sparks");
    }

    void OnCollisionStay(Collision collision) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);

        Instantiate(sparks, contactPoint, rotation, transform);
    }
}
