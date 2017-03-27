using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

public class UserInterface : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject planePrefab;

    GameStatus _gameStatus;
    ScenceStatus _scenceStatus;

    IUserInterface _uerInterface;
    IQueryStatus _queryStatus;


    // Use this for initialization
    void Start () {
        _gameStatus = GameStatus.ing;
        _scenceStatus = ScenceStatus.waiting;
        _uerInterface = GameScenceController.getGSController() as IUserInterface;
        _queryStatus = GameScenceController.getGSController() as IQueryStatus;
    }
	
	// Update is called once per frame
	void Update () {
        _gameStatus = _queryStatus.queryGameStatus();
        _scenceStatus = _queryStatus.queryScenceStatus();
		if (_gameStatus == GameStatus.ing)
        {
            if(_scenceStatus == ScenceStatus.waiting && Input.GetKeyDown("space"))
            {
                 _uerInterface.sendDisk();
            }
            if(_scenceStatus == ScenceStatus.shooting && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit) && hit.collider.gameObject.tag == "Disk")
                {
                    _uerInterface.destroyDisk(hit.collider.gameObject);
                }
            }
        }
	}
}
