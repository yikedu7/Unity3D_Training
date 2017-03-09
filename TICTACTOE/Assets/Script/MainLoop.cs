using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * MainLoop为业务层
 * 主要控制放置棋子，判断游戏状态
 */
public class MainLoop : MonoBehaviour {

    public GameObject OPrefab;
    public GameObject XPrefab;

    //State枚举类型储存游戏状态
    enum State
    {
        OGo = 0,
        XGo = 1,
        Over = 2
    }

    State _state;
    BoardModel _model;

    //传入点击的格子和当前游戏状态，放置棋子
    bool PlaceChess(Grid grid, State _state)
    {
        var ifSet = _model.set(grid.GridX, grid.GridY, _state == 0 ? ChessType.O : ChessType.X);
        if (ifSet == false)
        {
            return false;
        }

        var newChess = GameObject.Instantiate<GameObject>(_state == 0 ? OPrefab : XPrefab);
        newChess.transform.SetParent(grid.transform);
        newChess.transform.localScale = Vector3.one;
        var pos = grid.transform.localPosition;
        pos.x /= 120;
        pos.y /= 120;
        pos.z /= 120;
        newChess.transform.localPosition = pos;
        return true;
    }

    //重置游戏
    public void Reset()
    {
        _state = State.OGo;
        _model = new BoardModel();
    }

    // Use this for initialization
    void Start () {
        Reset();
	}

    public GameObject OWin;
    public GameObject XWin;

    //点击事件的处理
    public void OnClick(Grid grid)
    {
        //若当前为O棋回合
        if (_state == State.OGo)
        {
            if (PlaceChess(grid, _state) == true)
            {
                //若游戏胜利
                if(_model.checkWinner() == ChessType.O)
                {
                    _state = State.Over;

                    //提示游戏胜利
                    var newwin = GameObject.Instantiate<GameObject>(OWin);
                    newwin.transform.SetParent(gameObject.transform);
                    newwin.transform.localScale = Vector3.one;
                    Vector3 vec = new Vector3(-90, 40, 0);
                    newwin.transform.localPosition = vec;
                }
                //若未胜利则换X下棋
                else
                {
                    _state = State.XGo;
                }
            }
        }
        //若当前为X棋回合
        else if (_state == State.XGo)
        {
            if (PlaceChess(grid, _state) == true)
            {
                if (_model.checkWinner() == ChessType.X)
                {
                    _state = State.Over;
                    var newwin = GameObject.Instantiate<GameObject>(XWin);
                    newwin.transform.SetParent(gameObject.transform);
                    newwin.transform.localScale = Vector3.one;
                    Vector3 vec = new Vector3(-90, 40, 0);
                    newwin.transform.localPosition = vec;
                }
                else
                {
                    _state = State.OGo;
                }
            }
        }
    }
}
