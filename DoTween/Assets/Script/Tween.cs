using UnityEngine;

namespace myTween
{
    public class Tween
    {
        internal string tid;
        internal Vector3 target;
        internal float duration;
        internal bool isPause = false;
        internal bool isAutoKill = true;
        internal Coroutine coroutine = null;
        internal Transform transform = null;
        public delegate void OnComplete();
        internal OnComplete onComplete = null;

        //constructor
        public Tween(string tid, Vector3 target, float duration, Transform transform, Coroutine coroutine)
        {
            this.tid = tid;
            this.target = target;
            this.duration = duration;
            this.transform = transform;
            this.coroutine = coroutine;
            IDOTween.getInstance().Add(this);
        }

        //set the coroutine of tween
        public void setCoroutine(Coroutine coroutine)
        {
            this.coroutine = coroutine;
        }

        public void setAutoKill(bool isAutoKill)
        {
            this.isAutoKill = isAutoKill;
        }

        public void Play()
        {
            isPause = false;
        }

        public void Pause()
        {
            isPause = true;
        }

        public void Kill()
        {
            MonoBehaviour mono = transform.GetComponent<MonoBehaviour>();
            mono.StopCoroutine(coroutine);
            IDOTween.getInstance().Remove(this);
        }

        public void runOnComplete()
        {
            if (onComplete != null)
            {
                onComplete();
            }
            if (isAutoKill)
            {
                Kill();
            }
        }

        public void setOnComplete(OnComplete fun)
        {
            onComplete += fun;
        }
    }
}

