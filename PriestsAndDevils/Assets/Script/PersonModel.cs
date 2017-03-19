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

public enum onBoatState
{
    not = 0,
    left = 1,
    right = 2
}

public class PersonModel {

    public PersonStyle IPersonStyle { get; set; }
    public PersonState IPersonState { get; set; }
    public onBoatState IOnBoatState { get; set; }

}
