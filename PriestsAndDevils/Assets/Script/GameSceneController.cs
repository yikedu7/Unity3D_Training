using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour {

    public GameObject backgroundPrefab;
    public GameObject boatPrefab;
    public GameObject zombiePrefab;
    public GameObject sunflowerPrefab;
    public GameObject GoPrefab;
    public GameObject boat;
    public GameObject go;

    SenceModel _senceModel;

    const float distance = 1.2f;
    const int objCout = 6;


    public void Reset()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        _senceModel = new SenceModel();
        _senceModel.Reset();

        var background = GameObject.Instantiate<GameObject>(backgroundPrefab);
        background.transform.SetParent(gameObject.transform);

        for (int i = 0; i < objCout; i++)
        {
            if (i < 3)
            {
                var sunflower = GameObject.Instantiate<GameObject>(sunflowerPrefab);
                sunflower.transform.SetParent(gameObject.transform);

                var sunflowerClick = sunflower.GetComponent<Person>();
                sunflowerClick.gameSceneController = this;
                sunflowerClick.num = i;

                var position = sunflower.transform.localPosition;
                position.x -= distance * i;
                sunflower.transform.localPosition = position;
            }
            else
            {
                var zombie = GameObject.Instantiate<GameObject>(zombiePrefab);
                zombie.transform.SetParent(gameObject.transform);

                var zombieClcik = zombie.GetComponent<Person>();
                zombieClcik.gameSceneController = this;
                zombieClcik.num = i;

                var position = zombie.transform.localPosition;
                position.x += distance * (i - 3);
                zombie.transform.localPosition = position;
            }
        }

        boat = GameObject.Instantiate<GameObject>(boatPrefab);
        boat.transform.SetParent(gameObject.transform);

        go = GameObject.Instantiate<GameObject>(GoPrefab);
        go.transform.SetParent(gameObject.transform);
        go.GetComponent<GoClick>().gameSceneController = this;

    }

    public void PersonOnclick(Person _person)
    {
        PersonState _personState = _senceModel.getPersonModel(_person.num).IPersonState;
        BoatState _boatState = _senceModel.IBoatState;
        int _onBoatCout = _senceModel.onBoatCout();

        if (_boatState == BoatState.up && _personState == PersonState.up || _boatState == BoatState.down && _personState == PersonState.down)
        {
            if (_onBoatCout < 2)
            {
                _person.transform.SetParent(boat.transform);
                _senceModel.getPersonModel(_person.num).IPersonState = PersonState.onBoat;
            }
            if (_onBoatCout == 0)
            {
                _person.transform.localPosition = new Vector3(-0.4f, 0.4f, 0);
            }
            if (_onBoatCout == 1)
            {
                _person.transform.localPosition = new Vector3(0.4f, 0.4f, 0);
            }
        }
        if (_personState == PersonState.onBoat)
        {
            if (_boatState == BoatState.up)
            {
                _person.transform.SetParent(gameObject.transform);
                _person.transform.localPosition = _person.getStart();
                _senceModel.getPersonModel(_person.num).IPersonState = PersonState.up;
            }
            if (_boatState == BoatState.down)
            {
                _person.transform.SetParent(gameObject.transform);
                _person.transform.localPosition = _person.getArrive();
                _senceModel.getPersonModel(_person.num).IPersonState = PersonState.down;
                if (_senceModel.checkGameState() == GameState.win)
                {
                    
                }
            }
        }
    }

    public void goClick()
    {
        int _onBoatCout = _senceModel.onBoatCout();
        if (_onBoatCout != 0)
        {
            if (_senceModel.IBoatState == BoatState.up) 
                boat.transform.localPosition = new Vector3(-2,-1.4f,0);
            else
                boat.transform.localPosition = new Vector3(-2, -0.4f, 0);
            _senceModel.IBoatState = 1 - _senceModel.IBoatState;
        }
        if (_senceModel.checkGameState() == GameState.fail)
        {
            Reset();
        }
    }


    // Use this for initialization
    void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
