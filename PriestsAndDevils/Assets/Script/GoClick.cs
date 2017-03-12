using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoClick : MonoBehaviour
{

    public GameSceneController gameSceneController;

    private void OnMouseDown()
    {
        gameSceneController.goClick();
    }
}

