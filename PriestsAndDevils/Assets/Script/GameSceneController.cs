using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour {
    const float DISTANCE = 1.2f;
    const int OBJECTCOUNT = 6;
    public GameObject backgroundPrefab;
    public GameObject boatPrefab;
    public GameObject zombiePrefab;
    public GameObject sunflowerPrefab;
    public GameObject GoPrefab;
    public GameObject boat;
    public GameObject go;
    SenceModel _senceModel;

    //重置游戏
    public void Reset()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        /*创建游戏模型*/
        _senceModel = new SenceModel();
        _senceModel.Reset();

        /*创建所有游戏对象*/
        var background = GameObject.Instantiate<GameObject>(backgroundPrefab);
        background.transform.SetParent(gameObject.transform);
        /*循环创建游戏精灵*/
        for (int i = 0; i < OBJECTCOUNT; i++)
        {
            if (i < 3)
            {
                var sunflower = GameObject.Instantiate<GameObject>(sunflowerPrefab);
                sunflower.transform.SetParent(gameObject.transform);
                var sunflowerClick = sunflower.GetComponent<Person>();
                sunflowerClick.gameSceneController = this;
                sunflowerClick.num = i;
                var position = sunflower.transform.localPosition;
                position.x -= DISTANCE * i;
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
                position.x += DISTANCE * (i - 3);
                zombie.transform.localPosition = position;
            }
        }
        //初始化船对象
        boat = GameObject.Instantiate<GameObject>(boatPrefab);
        boat.transform.SetParent(gameObject.transform);
        //初始化Go按钮
        go = GameObject.Instantiate<GameObject>(GoPrefab);
        go.transform.SetParent(gameObject.transform);
        go.GetComponent<GoClick>().gameSceneController = this;
    }

    //处理点击精灵事件
    public void PersonOnclick(Person _person)
    {
        PersonModel _personModel = _senceModel.getPersonModel(_person.num);
        PersonState _personState = _personModel.IPersonState;
        BoatState _boatState = _senceModel.IBoatState;
        int _onBoatCout = _senceModel.onBoatCout();

        if (_boatState == BoatState.up && _personState == PersonState.up || _boatState == BoatState.down && _personState == PersonState.down)
        {
            if (_onBoatCout < 2)
            {
                _person.transform.SetParent(boat.transform);
                _personModel.IPersonState = PersonState.onBoat;
                if (_onBoatCout == 0 || (_onBoatCout == 1 && _senceModel.ifOnRight()))
                {
                    //moveToLeft(_person);
                    _personModel.IOnBoatState = onBoatState.left;
                }
                else if (_onBoatCout == 1 && !(_senceModel.ifOnRight()))
                {
                    //moveToRight(_person);
                    _personModel.IOnBoatState = onBoatState.right;
                }
            }
        }
        if (_personState == PersonState.onBoat)
        {
            _person.transform.SetParent(gameObject.transform);
            if (_boatState == BoatState.up)
            {
                //moveToStart(_person);
                _personModel.IPersonState = PersonState.up;
            }
            if (_boatState == BoatState.down)
            {
                //moveToArrive(_person);
                _personModel.IPersonState = PersonState.down;
                if (_senceModel.checkGameState() == GameState.win)
                {

                }
            }
        }

        //if (_boatState == BoatState.up && _personState == PersonState.up || _boatState == BoatState.down && _personState == PersonState.down)
        //{
        //    if (_onBoatCout < 2)
        //    {
        //        _person.transform.SetParent(boat.transform);
        //        _personModel.IPersonState = PersonState.onBoat;
        //    }
        //    if (_onBoatCout == 0)
        //    {
        //        _person.transform.localPosition = new Vector3(-0.4f, 0.4f, 0);
        //    }
        //    if (_onBoatCout == 1)
        //    {
        //        _person.transform.localPosition = new Vector3(0.4f, 0.4f, 0);
        //    }
        //}
        //if (_personState == PersonState.onBoat)
        //{
        //    if (_boatState == BoatState.up)
        //    {
        //        _person.transform.SetParent(gameObject.transform);
        //        _person.transform.localPosition = _person.start;
        //        _personModel.IPersonState = PersonState.up;
        //    }
        //    if (_boatState == BoatState.down)
        //    {
        //        _person.transform.SetParent(gameObject.transform);
        //        _person.transform.localPosition = _person.arrive;
        //        _personModel.IPersonState = PersonState.down;
        //        if (_senceModel.checkGameState() == GameState.win)
        //        {

        //        }
        //    }
        //}
    }

    //点击GO按钮
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

    void Start () {
        Reset();
	}
}
