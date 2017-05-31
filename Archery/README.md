# README

## 游戏简介

​	射箭小游戏是一款射击类小游戏，游戏规则十分简单：玩家点击屏幕上的任意位置，弓箭就会向着鼠标所指的方向发射（注意弓箭会受到重力的影响）。玩家射中靶心得5分，每往外一环少一分，该游戏没有输赢设置。

## 游戏展示

​	![](http://ompnv884d.bkt.clouddn.com/Archery.gif)

## 设计思路

​	这次项目还是使用熟悉的MVC架构。设计类图如下所示：

![](http://ompnv884d.bkt.clouddn.com/archeryuml.JPG)

## 开发过程

	#### (一)构建预设与素材

![](http://ompnv884d.bkt.clouddn.com/archery1.JPG)

​	先准备好游戏对象如箭靶、箭，设置的时候需要注意设置好`Rigidbody`属性和`Collider`属性。例如箭靶需要设置为`is Kinematic`避免它受到物理引擎的影响，而箭则需要设置`is Kinematic`为`false`并为其添加重力作用。至于`Collider`的设置，对于箭靶和箭头的圆柱体我选择了`Mesh Collider`，箭身则使用`Capsule Collider`，碰撞器的选择会影响到物理效果的精度，对后面计分功能也有影响，需要谨慎选择。另外，对于箭头与箭身的结合，我使用了`FixedJoint`组件，这在后文会详细介绍，在这里只要为箭头与箭身添加该组件即可。

#### (二)代码复用

​	在比较熟悉了MVC架构之后，开发速率会极大提升，因为之前使用的许多代码其实是可以复用的。例如`ScenceControl`类与`GameScenceController`类，我直接复用了上次打飞碟项目的代码，稍微修改一下类接口就可以立刻使用，开发效率得到很高的提升。对于`ScenceControl`类，我添加了一个`setArrow()`函数，如下：

```c#
GameObject setArrow()
{
    GameObject arrow = GameObject.Instantiate<GameObject>(arrowPrefab);
    GameObject head = GameObject.Instantiate<GameObject>(headPrefab);
    arrow.GetComponent<Rigidbody>().useGravity = false;
    arrow.GetComponent<FixedJoint>().connectedBody = head.GetComponent<Rigidbody>();
    head.GetComponent<FixedJoint>().connectedBody = arrow.GetComponent<Rigidbody>();
    return arrow;
}
```

​	需要注意的是这里使用了`FixedJoint`组件，下面是该组件的简介：

>**Fixed Joints** restricts an object's movement to be dependent upon another object. This is somewhat similar to **Parenting** but is implemented through physics rather than **Transform** hierarchy. The best scenarios for using them are when you have objects that you want to easily break apart from each other, or connect two object's movement without parenting.
>
>固定关节基于另一个物体来限制一个物体的运动。效果类似于父子关系，但是不是通过层级变换，而是通过物理实现的。使用它的最佳情境是当你有一些想要轻易分开的物体，或想让两个没有父子关系的物体一起运动。
>
>​																			— [Unity圣典](http://game.ceeger.com/) 

​	大致就是，`Fixed` `Joints`可以帮助我们实现两个物体一同运动的效果，需要注意的是这两个物体必须都是刚体。在创建`arrow`与`head`后需要设置`connectedBody`属性来实现绑定。

#### (三)通过射线实现射箭

​	这一部分代码我们同样熟悉了，与打飞碟中射出子弹的实现相似，这里不做赘述。代码如下：

```c#
void FixedUpdate () {
    if (Input.GetMouseButtonDown(0))
    {
        nowArrow = setArrow();
        Vector3 mp = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mp);
        nowArrow.GetComponent<Rigidbody>().useGravity = true;
        nowArrow.GetComponent<Rigidbody>().AddForce(ray.direction * 30, ForceMode.Impulse);
        nowArrow.AddComponent<ArrowComponent>();
        nowArrow = null;
    }
}
```

​	这次的实现我将这部分代码写在`FixedUpdate()`而不是`Update()`中，这样可以减少游戏消耗。

#### (四)碰撞的设置

​	通过`OnTriggerEnter()`可以监测触发器状态，当碰撞发射时会自动调用该函数，使用该函数我们可以实现“使箭留在箭靶” “计分”这两个需求。如下：

```c#
public class ArrowComponent : MonoBehaviour {
    private void OnTriggerEnter()
    {
      //取消动力学影响
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}

public class targetComponent : MonoBehaviour {
    private void OnTriggerEnter()
    {
      //调用加分函数
        GameScenceController.getGSController().addScore();
    }
}
```

​	最后再添加UI，挂载脚本，游戏就基本完成了。