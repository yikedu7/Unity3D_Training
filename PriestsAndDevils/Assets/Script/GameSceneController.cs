using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Action;


public class GameSceneController : MonoBehaviour
{
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
    ActionManager _actionManager = ActionManager.GetInstance();

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
        GameObject obj = _person.gameObject;
        PersonModel _personModel = _senceModel.getPersonModel(_person.num);
        PersonState _personState = _personModel.IPersonState;
        BoatState _boatState = _senceModel.IBoatState;
        int _onBoatCout = _senceModel.onBoatCout();

        if (_boatState == BoatState.up && _personState == PersonState.up)
        {
            if (_onBoatCout < 2)
            {
                _person.transform.SetParent(boat.transform);
                _personModel.IPersonState = PersonState.onBoat;
                if (_onBoatCout == 0 || (_onBoatCout == 1 && _senceModel.ifOnRight()))
                {
                    _actionManager.moveToLeftUp(obj);
                    _personModel.IOnBoatState = onBoatState.left;
                }
                else if (_onBoatCout == 1 && !(_senceModel.ifOnRight()))
                {
                    _actionManager.moveToRightUp(obj);
                    _personModel.IOnBoatState = onBoatState.right;
                }
            }
        }
        if (_boatState == BoatState.down && _personState == PersonState.down)
        {
            if (_onBoatCout < 2)
            {
                _person.transform.SetParent(boat.transform);
                _personModel.IPersonState = PersonState.onBoat;
                if (_onBoatCout == 0 || (_onBoatCout == 1 && _senceModel.ifOnRight()))
                {
                    _actionManager.moveToLeftDown(obj);
                    _personModel.IOnBoatState = onBoatState.left;
                }
                else if (_onBoatCout == 1 && !(_senceModel.ifOnRight()))
                {
                    _actionManager.moveToRightDown(obj);
                    _personModel.IOnBoatState = onBoatState.right;
                }
            }
        }
        if (_personState == PersonState.onBoat)
        {
            if (_boatState == BoatState.up)
            {
                _actionManager.moveToStart(obj);
                _personModel.IPersonState = PersonState.up;
                _personModel.IOnBoatState = onBoatState.not;
            }
            if (_boatState == BoatState.down)
            {
                _actionManager.moveToArrive(obj);
                _personModel.IPersonState = PersonState.down;
                _personModel.IOnBoatState = onBoatState.not;
                if (_senceModel.checkGameState() == GameState.win)
                {

                }
            }
            _person.transform.SetParent(gameObject.transform);
        }
    }

    //点击GO按钮
    public void goClick()
    {
        int _onBoatCout = _senceModel.onBoatCout();
        if (_onBoatCout != 0)
        {
            if (_senceModel.IBoatState == BoatState.up)
                _actionManager.boatMoveDown(boat);
            else
                _actionManager.boatMoveUp(boat);
            _senceModel.IBoatState = 1 - _senceModel.IBoatState;
        }
        if (_senceModel.checkGameState() == GameState.fail)
        {
            Reset();
        }
    }

    void Start()
    {
        Reset();
    }
}
