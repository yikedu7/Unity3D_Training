using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Manager;

public class UI : MonoBehaviour {

    GameObject scoreText;
    GameObject gameStatuText;
    IScore _getScore = GameScenceController.getGSController() as IScore;
    IQueryStatus _getGameStatu = GameScenceController.getGSController() as IQueryStatus;

	// Use this for initialization
	void Start () {
        scoreText = GameObject.Find("Score");
        gameStatuText = GameObject.Find("GameStatu");
    }
	
	// Update is called once per frame
	void Update () {
        string score = Convert.ToString(_getScore.getScore());
        if (_getGameStatu.queryGameStatus() == GameStatus.fail)
            gameStatuText.GetComponent<Text>().text = "fail!";
        else if (_getGameStatu.queryGameStatus() == GameStatus.win)
            gameStatuText.GetComponent<Text>().text = "win!";
        scoreText.GetComponent<Text>().text = "Score:" + score;
    }
}
