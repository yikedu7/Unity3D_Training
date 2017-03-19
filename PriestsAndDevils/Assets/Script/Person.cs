using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public GameSceneController gameSceneController;
    public int num;
    public Vector3 start { get; private set; }
    public Vector3 arrive { get; private set; }
   
    private void OnMouseDown()
    {
        gameSceneController.PersonOnclick(this);
    }

    private void Start()
    {
        start = gameObject.transform.position;
        arrive = new Vector3(start.x, start.y - 4, start.z);
    }
}
