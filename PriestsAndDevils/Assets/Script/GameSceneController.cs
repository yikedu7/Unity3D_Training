using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour {

    public GameObject backgroundPrefab;
    public GameObject boatPrefab;
    public GameObject zombiePrefab;
    public GameObject sunflowerPrefab;
    public GameObject goPrefab;
    const float distance = 1.2f;
    const int objCout = 6;


    public void Reset()
    {
        var background = GameObject.Instantiate<GameObject>(backgroundPrefab);
        background.transform.SetParent(gameObject.transform);

        for (int i = 0; i < objCout; i++)
        {
            if (i < 3)
            {
                var sunflower = GameObject.Instantiate<GameObject>(sunflowerPrefab);
                sunflower.transform.SetParent(gameObject.transform);

                var position = sunflower.transform.localPosition;
                position.x -= distance * i;
                sunflower.transform.localPosition = position;
            }
            else
            {
                var zombie = GameObject.Instantiate<GameObject>(zombiePrefab);
                zombie.transform.SetParent(gameObject.transform);

                var position = zombie.transform.localPosition;
                position.x += distance * (i - 3);
                zombie.transform.localPosition = position;
            }
        }

        var boat = GameObject.Instantiate<GameObject>(boatPrefab);
        boat.transform.SetParent(gameObject.transform);

        var go = GameObject.Instantiate<GameObject>(goPrefab);
        go.transform.SetParent(gameObject.transform);
    }

    // Use this for initialization
    void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
