using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonStyle
{
    sunflower = 0,
    zombie = 1
}

public enum PersonState
{
    onBoat = 0,
    up = 1,
    down = 2
}

public class PersonModel {

    PersonStyle _IPersonStyle;
    public PersonStyle IPersonStyle
    {
        get { return _IPersonStyle; }
        set { _IPersonStyle = value; }
    }

    PersonState _IPersonState;
    public PersonState IPersonState
    {
        get { return _IPersonState; }
        set { _IPersonState = value; }
    }

}
