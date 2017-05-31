using System.Collections;
using UnityEngine;

namespace myTween {
    public static class Extension
    {
        //MonoBehaviour的DoMove方法拓展
        public static IEnumerator DoMove(this MonoBehaviour mono, Tween tween)
        {
            Vector3 speed = (tween.target - tween.transform.position) / tween.duration;
            for (float f = tween.duration; f >= 0.0f; f -= Time.deltaTime)
            {
                tween.transform.Translate(speed * Time.deltaTime);
                yield return null;
                while (tween.isPause == true)
                {
                    yield return null;
                }
            }
            tween.runOnComplete();
        }

        //Transform的DoMove方法拓展
        public static Tween DoMove(this Transform transform, Vector3 target, float duration)
        {
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            Tween tween = new Tween("DoMove",target, duration, transform,null);
            Coroutine coroutine = mono.StartCoroutine(mono.DoMove(tween));
            tween.setCoroutine(coroutine);
            return tween;
        }

        //MonoBehaviour的DoScale方法拓展
        public static IEnumerator DoScale(this MonoBehaviour mono, Tween tween)
        {
            Vector3 speed = (tween.target - tween.transform.localScale) / tween.duration;
            for (float f = tween.duration; f >= 0.0f; f -= Time.deltaTime)
            {
                tween.transform.localScale = tween.transform.localScale + speed * Time.deltaTime;
                yield return null;
                while (tween.isPause == true)
                {
                    yield return null;
                }
            }
            tween.runOnComplete();
        }

        //Transform的DoScale方法拓展
        public static Tween DoScale(this Transform transform, Vector3 target, float time)
        {
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            Tween tween = new Tween("DoScale", target, time, transform, null);
            Coroutine coroutine = mono.StartCoroutine(mono.DoScale(tween));
            tween.setCoroutine(coroutine);
            return tween;
        }
    }
}
