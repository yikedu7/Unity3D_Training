using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Grid类为棋盘按钮
 * 挂载事件监听器，调用回调函数，执行mainLoop主函数
 */

public class Grid : MonoBehaviour
{
    //GridX，GridY为按钮坐标
    public int GridX;
    public int GridY;
    public MainLoop mainLoop;

    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            mainLoop.OnClick(this);
        });
    }
}
