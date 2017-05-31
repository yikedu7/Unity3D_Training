# README

## 项目简介

​	[DOTween](http://dotween.demigiant.com/)是一个用于实现插值动画(补间动画)的 Unity 插件。“插值”这个概念可能一些使用 AE 或 Flash 等动画软件的同学可能有所了解。简单来说，插值动画(补间动画)就是**在时间轴上定义两个不同时间的动画关键帧状态后，自动生成两个时间点之间所有帧动画**的**动画制作方法**。关于什么是插值动画(补间动画)，以及什么是帧动画在这里不做过多的解释，感兴趣的读者可以参考这一篇文章——[帧动画和补间动画](http://www.jianshu.com/p/5163789b1591)。

​	这个项目主要是利用C#中的拓展方法来实现一个简单的DOTween插件效果，实现过程涉及到拓展方法、协程等知识点。

## 设计参考

​	DOTween是一个开源插件，可以在GitHub上找到这个项目：[DOTween - GitHub](https://github.com/Demigiant/dotween)。官方实现的方式较为复杂，我在这里参考了其架构设计，将项目分成：`Tween`、`IDOTween`、`Extension`。

## 效果展示

​	![](http://ompnv884d.bkt.clouddn.com/DOTween.gif)

## 开发过程

#### （一）Tween.cs：补间动画类

​	定义一个简单的补间动画类，需要注意的是每个补间动画都包括一个协程。通过使用协程，我们的动画函数就可以实现不在一帧里完成，而是逐帧调用，每帧执行一部分。这符合动画实现的需求。

```c#
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
```

#### （二）IDOTween.cs：动画工厂类

​	工厂类主要通过一个`List`来储存和管理所有的补间动画对象，同时也定义了一些基础的增删改查的基础函数。主要是注意控制单实例。

```c#
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
```

#### （三）Extension.cs：拓展方法类

​	拓展方法类主要是通过C#的拓展方法机制，对`MonoBehaviour`与`Transform`类拓展控制`Tween`对象的一系列方法。这样用户只需导入已定义的命名空间`myTween`就可以拓展这些类方法。

```c#
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

```

#### （四）测试脚本及测试

 	写一个简单的插件测试脚本(注意导入命名空间)，在unity中挂载到一个游戏对象中，就可以测试插件的运行情况了：

```c#
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
```