using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowComponent : MonoBehaviour {
    private void OnTriggerEnter()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}
