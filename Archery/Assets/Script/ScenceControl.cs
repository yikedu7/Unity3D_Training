using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

public class ScenceControl : MonoBehaviour {

    public GameObject targetPrefab;
    public GameObject arrowPrefab;
    public GameObject headPrefab;
    public GameObject wallPrefab;

    private GameObject nowArrow;

    // Use this for initialization
    void Start () {
        GameObject target = GameObject.Instantiate<GameObject>(targetPrefab);
        GameObject wall = GameObject.Instantiate<GameObject>(wallPrefab);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        {
            nowArrow = setArrow();
            Vector3 mp = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mp);
            nowArrow.GetComponent<Rigidbody>().useGravity = true;
            nowArrow.GetComponent<Rigidbody>().AddForce(ray.direction * 30, ForceMode.Impulse);
            nowArrow.AddComponent<ArrowComponent>();
            nowArrow = null;
        }
    }

    GameObject setArrow()
    {
        GameObject arrow = GameObject.Instantiate<GameObject>(arrowPrefab);
        GameObject head = GameObject.Instantiate<GameObject>(headPrefab);
        arrow.GetComponent<Rigidbody>().useGravity = false;
        arrow.GetComponent<FixedJoint>().connectedBody = head.GetComponent<Rigidbody>();
        head.GetComponent<FixedJoint>().connectedBody = arrow.GetComponent<Rigidbody>();
        return arrow;
    }
}
