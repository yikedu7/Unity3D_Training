
using UnityEngine;

//ChessType声明一个枚举类型，用于储存棋子类型
public enum ChessType
{
    None = 0,
    O = 1,
    X = 2
}

/*
 * BoardModel类为棋盘抽象模型
 * 主体是一个3*3的二维数组，储存棋盘的棋子状况
 */
public class BoardModel{
    private const int chessCount = 3;
    private const int winChessCount = 3;

    private ChessType[,] _data = new ChessType[chessCount, chessCount];

    public ChessType get(int x, int y)
    {
        return _data[x, y];
    }

    public bool set(int x, int y, ChessType chessType)
    {
        if (_data[x, y] == 0)
        {
            _data[x, y] = chessType;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reset ()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _data[i, j] = 0;
            }
        }
    }

    //判断是否达到胜利条件，返回胜利一方的棋子，若无胜利返回None
    public ChessType checkWinner()
    {
        //横向
        for (int i = 0; i < 3; ++i)
        {
            if (_data[i, 0] != 0 && _data[i, 0] == _data[i, 1] && _data[i, 1] == _data[i, 2])
            {
                return _data[i, 0];
            }
        }
        //纵向
        for (int j = 0; j < 3; ++j)
        {
            if (_data[0, j] != 0 && _data[0, j] == _data[1, j] && _data[1, j] == _data[2, j])
            {
                return _data[0, j];
            }
        }
        //斜向
        if (_data[1, 1] != 0 &&
            _data[0, 0] == _data[1, 1] && _data[1, 1] == _data[2, 2] ||
            _data[0, 2] == _data[1, 1] && _data[1, 1] == _data[2, 0])
        {
            return _data[1, 1];
        }
        return 0;
    }
}
