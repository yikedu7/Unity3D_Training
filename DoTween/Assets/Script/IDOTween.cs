using System.Collections.Generic;
using UnityEngine;

namespace myTween {
    public class IDOTween
    {
        private static List<Tween> dotweenList = new List<Tween>();
        private static IDOTween dotween = null;

        //控制单实例
        public static IDOTween getInstance()
        {
            if (dotween == null)
            {
                dotween = new IDOTween();
            }
            return dotween;
        }

        public void Add(Tween d)
        {
            dotweenList.Add(d);
        }

        public void Remove(Tween d)
        {
            dotweenList.Remove(d);
        }

        public static void Play(string id)
        {
            foreach (Tween t in dotweenList)
            {
                if (t.tid == id)
                {
                    t.Play();
                }
            }
        }

        public static void Play(Transform transform)
        {
            foreach (Tween d in dotweenList)
            {
                if (d.transform == transform)
                {
                    d.Play();
                }
            }
        }

        public static void PlayAll()
        {
            foreach (Tween d in dotweenList)
            {
                d.Play();
            }
        }

        public static void Pause(string id)
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                if (dotweenList[i].tid == id)
                {
                    dotweenList[i].Pause();
                }
            }
        }

        public static void Pause(Transform transform)
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                if (dotweenList[i].transform == transform)
                {
                    dotweenList[i].Pause();
                }
            }
        }

        public static void PauseAll()
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                dotweenList[i].Pause();
            }
        }

        public static void Kill(string id)
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                if (dotweenList[i].tid == id)
                {
                    dotweenList[i].Kill();
                }
            }
        }

        public static void Kill(Transform transform)
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                if (dotweenList[i].transform == transform)
                {
                    dotweenList[i].Pause();
                }
            }
        }

        public static void KillAll()
        {
            for (int i = dotweenList.Count - 1; i >= 0; i--)
            {
                dotweenList[i].Kill();
            }
        }
    }
}


