using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    ing = 0,
    win = 1,
    fail = 2
}

public enum BoatState
{
    up = 0,
    down = 1
}

public class SenceModel  {
    
    const int OBJECTNUM = 6;//六个游戏对象
    public BoatState IBoatState { get; set; }
    PersonModel[] allPerson;

    //初始化
    public void Reset()
    {
        allPerson = new PersonModel[OBJECTNUM];
        for (int i = 0; i < OBJECTNUM; i++)
        {
            allPerson[i] = new PersonModel();
            allPerson[i].IPersonState = PersonState.up;
            if (i < 3)
            {
                allPerson[i].IPersonStyle = PersonStyle.sunflower;
            }
            else
            {
                allPerson[i].IPersonStyle = PersonStyle.zombie;
            }
        }
    }

    //get访问器获取指定游戏对象
    public PersonModel getPersonModel(int num)
    {
        return allPerson[num];
    }

    //关于船的判断方法
    public int onBoatCout()
    {
        int onBoatCout = 0;
        foreach (var person in allPerson)
        {
            if (person.IPersonState == PersonState.onBoat)
                onBoatCout++;
        }
        return onBoatCout;
    }
    public bool ifOnRight()
    {
        bool onRight = false;
        foreach (var person in allPerson)
        {
            if (person.IOnBoatState == onBoatState.right)
                onRight = true;
        }
        return onRight;
    }

    //判断游戏状态
    public GameState checkGameState()
    {
        //声明四个变量储存植物和僵尸的数量
        int upSunflower = 0, downSunflower = 0;
        int upZombie = 0, downZombie = 0;

        //遍历对象数组
        foreach (var person in allPerson)
        {
            if (IBoatState == BoatState.up)
            {
                if (person.IPersonState != PersonState.down)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        upSunflower++;
                    else
                        upZombie++;
                }
                else if (person.IPersonState == PersonState.down)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        downSunflower++;
                    else
                        downZombie++;
                }
            }
            if (IBoatState == BoatState.down)
            {
                if (person.IPersonState != PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        downSunflower++;
                    else
                        downZombie++;
                }
                else if (person.IPersonState == PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        upSunflower++;
                    else
                        upZombie++;
                }
            }
        }
        //判断游戏状态
        if ((upZombie > upSunflower && upSunflower != 0) ||
                  downZombie > downSunflower && downSunflower != 0)
            return GameState.fail;

        foreach (var person in allPerson)
        {
            if (person.IPersonState == PersonState.up || person.IPersonState == PersonState.onBoat)
                return GameState.ing;
        }
        return GameState.win;
    }
}
