using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairZoneController : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        MeshDeformation md = collider.gameObject.GetComponentInParent<MeshDeformation>();
        md.SetIsRepairing(true);
    }

    void OnTriggerExit(Collider collider) {
        MeshDeformation md= collider.gameObject.GetComponentInParent<MeshDeformation>();
        md.SetIsRepairing(false);
    }
}
