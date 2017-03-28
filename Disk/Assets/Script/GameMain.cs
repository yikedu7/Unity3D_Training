using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

namespace Com.Manager
{
    public interface IUserInterface
    {
        void sendDisk();
        void destroyDisk(GameObject disk);
    }

    public enum GameStatus
    {
        ing = 0,
        win = 1,
        fail = 2
    }

    public enum ScenceStatus
    {
        waiting = 0,
        shooting = 1
    }

    public interface IQueryStatus
    {
        GameStatus queryGameStatus();
        ScenceStatus queryScenceStatus();
    }

    public interface setStatus
    {
        void setGameStatus(GameStatus _gameStatus);
        void setScenceStatus(ScenceStatus _scenceStatus);
    }

    public interface IScore
    {
        void addScore();
        void subScore();
        int getScore();
    }

    public class GameScenceController : IUserInterface, IQueryStatus, setStatus
    {
        public int sendDiskCount { get; private set; }
        private static GameScenceController _gameScenceController;
        private GameStatus _gameStatus;
        private ScenceStatus _scenceStatus;
        public Scence _scence;
        private DiskFactory _diskFactory = DiskFactory.getFactory();

        public static GameScenceController getGSController()
        {
            if (_gameScenceController == null)
                _gameScenceController = new GameScenceController();
            return _gameScenceController;
        }

        public void sendDisk()
        {
            int diskCount = _scence.diskCount;
            var diskList = _diskFactory.prepareDisks(diskCount);
            _scence.sendDisk(diskList);
        }

        public void destroyDisk(GameObject disk)
        {
            _scence.destroyDisk(disk);
            _diskFactory.recycleDisk(disk);
        }

        public GameStatus queryGameStatus() { return _gameStatus; }
        public ScenceStatus queryScenceStatus() { return _scenceStatus; }

        public void setGameStatus(GameStatus _gameStatus) { this._gameStatus = _gameStatus; }
        public void setScenceStatus(ScenceStatus _scenceStatus) { this._scenceStatus = _scenceStatus; }
    }
}

public class GameMain : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameScenceController.getGSController()._scence = this.gameObject.AddComponent<Scence>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
