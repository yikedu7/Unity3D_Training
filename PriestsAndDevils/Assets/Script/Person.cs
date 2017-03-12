using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public GameSceneController gameSceneController;
    public int num;

    Vector3 start;
    public Vector3 getStart()
    {
        return start;
    }
    Vector3 arrive;
    public Vector3 getArrive()
    {
        return arrive;
    }

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
