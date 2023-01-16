using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour {
    [SerializeField]
    Transform target;

    [SerializeField]
    bool rotate;

    // Update is called once per frame
    void Update() {
        Vector3 targetPosition = target.position;

        targetPosition.y = transform.position.y;
        transform.position = targetPosition;

        if (rotate) {
            Vector3 targetRotation = target.rotation.eulerAngles;

            targetRotation.x = transform.rotation.eulerAngles.x;
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}
