using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

public class targetComponent : MonoBehaviour {
    private void OnTriggerEnter()
    {
        GameScenceController.getGSController().addScore();
    }
}
