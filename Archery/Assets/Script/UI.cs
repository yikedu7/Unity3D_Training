using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Manager;

public class UI : MonoBehaviour {

    public GameObject scoreText;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        string score = Convert.ToString(GameScenceController.getGSController().getScore());
        scoreText.GetComponent<Text>().text = "Score:" + score;
    }
}
