using UnityEngine;
using myTween;

public class DOTweenTest : MonoBehaviour {

    Tween tween;

    private void Start()
    {
        transform.DoMove(new Vector3(5f, 5f, 5f), 5f);
        transform.DoScale(new Vector3(5f, 5f, 5f), 5f);
    }
}
