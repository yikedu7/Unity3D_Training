using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

public class GameModel : IScore
{

    public int score { get; private set; }
    private static GameModel _gameModel;

    public static GameModel getGameModel()
    {
        if (_gameModel == null)
        {
            _gameModel = new GameModel();
            _gameModel.score = 0;
        }
        return _gameModel;
    }

    public void addScore()
    {
        score++;
        Debug.Log(score);
    }

    public int getScore() { return score; }

}
