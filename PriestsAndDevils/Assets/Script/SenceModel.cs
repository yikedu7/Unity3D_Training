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

    const int objCout = 6;

    GameState IGameState = GameState.ing;
    PersonModel[] allPerson = new PersonModel[objCout];
    BoatState IBoatState = BoatState.up;

    public GameState checkGameState()
    {
        int upCout = 0 , downCout = 0;

        foreach(var person in allPerson)
        {
            if (IBoatState == BoatState.up)
            {
                if (person.IPersonState != PersonState.down)
                {
                    
                }
                else if (person.IPersonState == PersonState.down)
                {
                   
                }
            }
            if (IBoatState == BoatState.down)
            {
                if (person.IPersonState != PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        downCout++;
                    else
                        downCout--;
                }
                else if (person.IPersonState == PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        upCout++;
                    else
                        upCout--;
                }
            }
        }

        if (upCout < 0 || downCout < 0)
        {
            return GameState.fail;
        }
        else if ()

        return 0;
    }

    public void changBoatState()
    {
        IBoatState = 1 - IBoatState;
    }
}
