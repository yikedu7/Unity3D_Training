using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

public class GameModel :IScore {

    public int score { get; private set; }
    public int round { get; private set; }
    private static GameModel _gameModel;

    public static GameModel getGameModel()
    {
        if (_gameModel == null)
        {
            _gameModel = new GameModel();
            _gameModel.round = 1;
        }
        return _gameModel;
    }

    public void addScore()
    {
        score += 10;
        if (checkUpdate())
        {
            this.round++;
            GameScenceController.getGSController().update();
        }
    }

    public void subScore()
    {
        score -= 10;
        if(score <= 0)
        {
            GameScenceController.getGSController().setGameStatus(GameStatus.fail);
        }
    }

    public bool checkUpdate()
    {
        if (score >= round * 20)
            return true;
        return false; 
    }

    public int getScore() { return GameModel.getGameModel().score; }

}
