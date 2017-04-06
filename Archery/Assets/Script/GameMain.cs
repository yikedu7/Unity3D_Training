using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

namespace Com.Manager
{
    public interface IScore
    {
        void addScore();
        int getScore();
    }

    public class GameScenceController : IScore
    {
        private static GameScenceController _gameScenceController;
        public static GameScenceController getGSController()
        {
            if (_gameScenceController == null)
                _gameScenceController = new GameScenceController();
            return _gameScenceController;
        }

        public void addScore() { GameModel.getGameModel().addScore(); }
        public int getScore() { return GameModel.getGameModel().score; }
    }
}

public class GameMain : MonoBehaviour
{
}

