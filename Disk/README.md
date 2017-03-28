# README

### 游戏简介

打飞碟小游戏是一款射击类小游戏，游戏规则如下：

- 每回合开始，玩家按“空格键”发射若干飞碟（飞碟数量随回合数增加而增加），玩家通过鼠标控制点击，击中飞碟时获得10分。

- 若飞碟直到掉落在地还未被玩家击中，则玩家扣除10分。

- 若玩家分数低于或等于0分时，游戏失败。

- 玩家分数每达到一定分数（20分、40分等），则关卡升级。发射飞碟数量增多，发射速度加快。

### 游戏展示

![](http://ompnv884d.bkt.clouddn.com/Disk.gif)

### 设计思路

​	这次游戏设计继续采用MVC架构，根据自顶向下的设计思路，我首先考虑最顶层的控制类`GameScenceController`，它应该以**单实例**形式存在，并且通过对其它类提供功能不同的接口来实现向类转达信息。所以从功能考虑，`GameScenceController`实现了`IUserInterface`（用户操作接口）, `IQueryStatus`（状态查询接口）, `setStatus`（状态设置接口）, `IScore`（分数相关功能接口），这样我们就完成了`Control`层的设计。

​	`Model`层负责游戏逻辑，这次游戏逻辑较为简单，所以我用`GameModel`类来实现控制游戏分数、判断游戏输赢的逻辑功能。

​	`View`层较为复杂。首先需要一个场景类`Scence`来**设置飞碟的数量大小速度等属性、控制发射飞碟和隐藏飞碟，并且它要负责判断场景中的飞碟状态**并告知控制类。其次，为了实现飞碟对象的重复使用，使用工厂类`DiskFactory`类**创建和回收飞碟**。最后，UI部分需要提供两个类：一个`UserInterface`类负责**处理用户的操作**，`UI`类负责**实现游戏的UI视图**。

### 开发过程

#### (一)理解“接口”与使用接口

​	由于之前做的更多是一些C和C++的小项目，对于C#这种纯面向对象语言中“接口”的概念总觉得没办法很好地理解，相信也有许多新手和我有相同的困惑。通过这次项目的开发经历，我觉得自己已经能较完整的理解“接口”这个概念了。下面结合代码来谈谈我个人对“接口”设计的理解：

```c#
public class GameScenceController : IUserInterface, IQueryStatus, setStatus, IScore
{
  //...
}

public class UserInterface : MonoBehaviour
{
	//...
	void Start()
	{
      	//将控制类实例转化为特定的接口类型
    	_uerInterface = GameScenceController.getGSController() as IUserInterface;
    	_queryStatus = GameScenceController.getGSController() as IQueryStatus;
    	_changeScore = GameScenceController.getGSController() as IScore;
	}
}

```

​	像许多新手一样，此前我对接口的理解停留在**重要性**上——使用接口可以使代码可读性更强，结构更加清晰。其实，使用接口在架构设计的层面上看，有一定的**必要性**。就像上面的代码所示，对于`UserInterface`类，他需要向控制类获取游戏状态、分数的相关信息，同时需要向控制类传达用户操作信息，但是它**并不需要也不应该得到设置游戏状态的功能**。像这种，需要向一个类提供部分功能的情况，使用接口就能容易地实现，只要将**控制类实例转化为特定的接口类型**，就能限制其提供的功能。这符合面向对象设计的封装性原则。

#### （二）关于Unity刚体—— `Rigidbody`

​	当Unity中的一个对象被添加了`Rigidbody`组件，它便成为了一个“刚体”，即Unity会对其应用物理引擎，于是对象便会像现实物体一样存在受到力的作用。这次开发过程在设置`Disk`时简单的使用了一下`Rigidbody`组件。

```c#
public void sendDisk(List<GameObject> usingDisks)
{
    this.usingDisks = usingDisks;
    this.Reset(round);
    for (int i = 0; i < usingDisks.Count; i++)
    {
        var localScale = usingDisks[i].transform.localScale;
        usingDisks[i].transform.localScale = new Vector3(localScale.x * diskScale, localScale.y * diskScale, localScale.z * diskScale);
        usingDisks[i].GetComponent<Renderer>().material.color = diskColor;
        usingDisks[i].transform.position = new Vector3(startPosition.x, startPosition.y + i, startPosition.z);
      	//使用Rigidbody
        Rigidbody rigibody;
        rigibody = usingDisks[i].GetComponent<Rigidbody>();
        rigibody.WakeUp();
        rigibody.useGravity = true;
        rigibody.AddForce(startDiretion * Random.Range(diskSpeed * 5, diskSpeed * 8) / 5, ForceMode.Impulse);
        GameScenceController.getGSController().setScenceStatus(ScenceStatus.shooting);
    }
}

public void destroyDisk(GameObject disk)
{
    disk.GetComponent<Rigidbody>().Sleep();
    disk.GetComponent<Rigidbody>().useGravity = false;
    disk.transform.position = new Vector3(0f, -99f, 0f);
}
```

​	结合这次的工厂回收机制，一个Disk使用完毕后并不是将它销毁，而是暂时隐藏等待再次使用。这样的情况下，我们将Disk暂时保存时，需要暂时让其`Rigidbody`不工作。原因很简单，如果不停用其`Rigidbody`，那么Disk会一直受到力的作用而不停的运动，这显然是会消耗游戏性能的，所以我们不希望这种情况出现。这里我选择了使用`Rigidbody.Sleep()`和`Rigidbody.WakeUp()`来停用和启用`Rigidbody`组件。

#### （三）再谈工厂模式

​	上次制作简单工厂项目时曾小试过一次工厂模式的使用，这次在项目中我同样也引入工厂模式来创建和回收飞碟。

```c#
public class DiskFactory : System.Object
{
    public GameObject diskPrefab;

    private static DiskFactory _diskFactory;
    List<GameObject> usingDisks;
    List<GameObject> uselessDisks;

    public static DiskFactory getFactory()
    {
        if (_diskFactory == null)
        {
            _diskFactory = new DiskFactory();
            _diskFactory.uselessDisks = new List<GameObject>();
            _diskFactory.usingDisks = new List<GameObject>();
        }
        return _diskFactory;
    }

    public List<GameObject> prepareDisks(int diskCount)
    {
        for (int i = 0; i < diskCount; i++)
        {
            if (uselessDisks.Count == 0)
            {
                GameObject disk = GameObject.Instantiate<GameObject>(diskPrefab);
                usingDisks.Add(disk);
            }
            else
            {
                GameObject disk = uselessDisks[0];
                uselessDisks.RemoveAt(0);
                usingDisks.Add(disk);
            }
        }
        return this.usingDisks;
    }

    public void recycleDisk(GameObject disk)
    {
        int index = usingDisks.FindIndex(x => x == disk);
        uselessDisks.Add(disk);
        usingDisks.RemoveAt(index);
    }
}
```

​	这次的工厂模式实现较为完整，使用两个`List`——`usingDisks`和`uselessDisks`来分别储存正在使用和使用完毕的Disk。需要注意的是，由于`DiskFactory`并非是继承`MonoBehaviour`的类，所以无法直接导入预设，只能在其它类中导入`Disk`预设再将其传给`DiskFactory`。