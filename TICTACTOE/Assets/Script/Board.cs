using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Board类为棋盘实体
 * 主要功能是创建并初始化棋盘及按钮
 */
public class Board : MonoBehaviour {

    public GameObject GridPrefab;
    const float size = 120;//每一格的大小
    const int GridCount = 3;//3*3棋盘

    //重置棋盘
    public void Reset()
    {
        foreach(Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //初始化一个MainLoop对象
        var mainLoop = GetComponent<MainLoop>();
        mainLoop.Reset();

        //创建并初始化3*3按钮
        for (int i = 0; i < GridCount; i++)
        {
            for (int j = 0; j < GridCount; j++)
            {
                //创建一个按钮
                var gridObject = GameObject.Instantiate<GameObject>(GridPrefab);
                gridObject.transform.SetParent(gameObject.transform);
                gridObject.transform.localScale = Vector3.one;

                //控制按钮的位置分布
                var pos = gridObject.transform.localPosition;
                pos.x = pos.x + (j - 1) * size;
                pos.y = pos.y - (i - 1) * size;
                pos.z = 0;
                gridObject.transform.localPosition = pos;

                //初始化该按钮
                var grid = gridObject.GetComponent<Grid>();
                grid.GridX = j;
                grid.GridY = i;
                grid.mainLoop = mainLoop;
            }
        }
    }

    // Use this for initialization
    void Start () {
        Reset();	
	}
}
