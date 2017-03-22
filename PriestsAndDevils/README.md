# README

###  V2.0更新说明

1. 使用动作管理器，实现动作分离
2. 修改了精灵上船位置的BUG

------

### 游戏简介

​	《植物与僵尸》是一款模仿《牧师与恶魔》的益智小游戏。规则与《牧师与恶魔》相似，只不过牧师在游戏中变成了植物，而恶魔变成了僵尸。

​	游戏规则简介如下：

> 成功条件：所有的植物与僵尸都完成过河。
>
> 制约条件：
>
> 1. 若河岸的某一边，僵尸的数目大于植物的数目，则游戏失败。**注意：船上的植物与僵尸也会计数, 若船在靠下方河岸，则计入下方河岸数目，反之亦然。**
> 2. 船只能搭载两个植物或僵尸。
> 3. 船上需要有至少一人才能使船移动。
>
> 操作方法：点击植物或僵尸使其上船，点击GO按键使船移动。

​	游戏效果：

![](http://ompnv884d.bkt.clouddn.com/GAME.gif-1)

### 参考资料

​	Simba_Scorpio的博客文章：http://blog.csdn.net/simba_scorpio/article/details/50846520

​	Unity官方文档：https://docs.unity3d.com/ScriptReference/

​	游戏蛮牛文档：http://docs.manew.com/index.html

### 游戏架构

​	游戏采用MVC架构来设计，主要目录如下：

![](http://ompnv884d.bkt.clouddn.com/%E6%8D%95%E8%8E%B73.JPG)

​	其中`GameSceneController`为控制类，负责生成以及控制所有游戏对象；`SenceModel`和`PersonModel`为模型类，主要负责整体和局部的逻辑实现；`Person`与`GoClick`为用户接口类，主要实现每个游戏精灵以及GO按钮的用户交互。

### 重点展示

​	关于逻辑实现，考虑的是数据结构尽量从简，以减少游戏消耗。所以这里采用最朴素的对象数组来储存逻辑对象。下面是`SenceModel`的关于这部分的部分代码：

```c#
public class SenceModel  {
	//六个游戏对象
    const int objCout = 6;
  	//游戏对象数组
    PersonModel[] allPerson = new PersonModel[objCout];
	//初始化
    public void Reset()
    {
        for (int i = 0; i < objCout; i++)
        {
            allPerson[i] = new PersonModel();
            allPerson[i].IPersonState = PersonState.up;
            if (i < 3)
            {
                allPerson[i].IPersonStyle = PersonStyle.sunflower;
            }
            else
            {
                allPerson[i].IPersonStyle = PersonStyle.zombie;
            }
        }
    }
    //get访问器获取指定游戏对象
    public PersonModel getPersonModel(int num)
    {
        return allPerson[num];
    }
}
```

​	游戏胜利判断较为简单，通过遍历上述对象数组`allPerson`，来分别获取上方河岸和下方河岸各自植物和僵尸的数量，然后作简单逻辑判断即可：

```c#
public GameState checkGameState() //GameState是自定义的一个表示游戏状态的枚举类型
    {
  		//声明四个变量储存植物和僵尸的数量
        int upSunflower = 0, downSunflower = 0;
        int upZombie = 0, downZombie = 0;
		//遍历对象数组
        foreach(var person in allPerson)
        {
            if (_IBoatState == BoatState.up)
            {
                if (person.IPersonState != PersonState.down)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        upSunflower++;
                    else
                        upZombie++;
                }
                else if (person.IPersonState == PersonState.down)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        downSunflower++;
                    else
                        downZombie++;
                }
            }
            if (_IBoatState == BoatState.down)
            {
                if (person.IPersonState != PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        downSunflower++;
                    else
                        downZombie++;
                }
                else if (person.IPersonState == PersonState.up)
                {
                    if (person.IPersonStyle == PersonStyle.sunflower)
                        upSunflower++;
                    else
                        upZombie++;
                }
            }
        }
		//判断游戏状态
        if ((upZombie > upSunflower && upSunflower != 0) ||
                  downZombie > downSunflower && downSunflower != 0)
            return GameState.fail;

        foreach (var person in allPerson)
        {
            if (person.IPersonState == PersonState.up || person.IPersonState == PersonState.onBoat)
                return GameState.ing;
        }
        return GameState.win;
    }
```

​	至于对于游戏对象的控制，需要注意的是对上船/下船动作的条件限制：例如船不在对应的岸边，对象是无法上船的；船上的对象已有2个也是无法上船；上船时需要控制对象的位置，若船上已有人则需要到有空位的位置...对象控制部分新手容易忽视的一个问题是：船上的对象要随船移动，所以需要将船上的对象设置为船对象的子对象。同理，下船的适合则将该对象的父对象重新设置为`gameObject`。

```c#
_person.transform.SetParent(boat.transform);
```

### 开发心得

​	由于这是我第一次相对独立开发的Unity游戏项目（上次五子棋项目基本按照慕课网的五子棋教程实现），所以还是遇到了很多新手开发者遇到的坑，再次做下记录。

#### （一）`Canvas`带来的困惑

​	没有完整接触过Unity组件常常会受`Canvas`影响，当创建一个`Button`时自动会生成一个`Canvas`作为其父对象。其实`Canvas`的主要功能就是负责UI布局和渲染的抽象空间，所有的UI都必须在此元素下。通过设置其`UI Scale Mode`可以实现游戏程序的自适应屏幕缩放。这里不做过多赘述，作者的`Canvas`设置如下图，仅供参考。需要注意的是用一个场景下所有的UI对象最好都声明在同一个`Canvas`父对象之下，否则可能会发生按钮click失效这种情况。

![](http://ompnv884d.bkt.clouddn.com/%E6%8D%95%E8%8E%B74.JPG)

#### （二）对MVC架构的设计

​	架构的设计对后期开发效率的影响十分巨大，这次我因为架构并不合理而踩了许多坑。首先，我对控制类`GameSceneController`没有做好单例控制，这一点在大型游戏开发中是很不对的，会带来对性能要求的提高。其次我没有意识到将`GameSceneController`声明到一个自定义的公共命名空间能够带来的便利，如下是一个比较好的示范：

```c#
namespace Com.Mygame {  
      
    public class GameSceneController: System.Object {  
          
        private static GameSceneController _instance;  
        private BaseCode _base_code;  //底层逻辑类
          
        public static GameSceneController GetInstance() {  
            if (null == _instance) {  
                _instance = new GameSceneController();  
            }  
            return _instance;  
        }  
          
        public BaseCode getBaseCode() {  
            return _base_code;  
        }  
          
        internal void setBaseCode(BaseCode bc) {  
            if (null == _base_code) {  
                _base_code = bc;  
            }  
        }  
    }  
}  
```

​	将`GameSceneController`声明到一个自定义的公共命名空间，其它脚本只要添加`using Com.Mygame`就看以使用控制类对象了，而且也能将我们自定义的类与系统类区分，也便于大型开发项目的管理。这次我由于没有这样做，导致在后期开发中添加了许多冗余繁杂的类接口来保证功能需求，带来了许多麻烦。

#### （三）时时刻刻注意实例化与初始化

​	基本上，每个类都需要一个初始化函数，来初始化所有字段属性。这一点新手开发者虽然知道但常常会遗忘。其次，对于某些类如果没有设置声明时自动初始化，要记得实例化的同时手动做好初始化。这种问题虽然能在后期Debug中比较容易地发现，但是极大地影响开发者的效率。

------

## V2.0相关

### 关于动作分离

​	使用简单的工厂模式，其原理时：添加动作管理器`ActionManager`，`ActionManager`设置分配动作给游戏对象的方法（`moveToLeftUp`、`moveToRightUp`等）。具体的分配动作的方法是：自定义一个对象组件`MoveToAction`，通过向对象添加组件和删除组件的方式来控制游戏对象的动作进行和动作结束。动作分离的好处是：代码结构清晰，`GameSceneController`不再需要了解动作的具体实现，而只需要通知`ActionManager`想特定的对象添加特定的动作即可。在大型的游戏工程中，一个动作的实现往往是相对复杂的，所以动作分离十分重要。

### 实现过程

#### （一）定义`ActionManager`类

​	定义`ActionManager`类，并添加需要用到的静态字段，用于表示动作的到达位置与速度。

```c#
    public class ActionManager : System.Object
    {
        private static ActionManager _IActionManager;
        static Vector3 LEFTUP = new Vector3(-2.5f, 0.2f, 0);
        static Vector3 RIGHTUP = new Vector3(-1.5f, 0.2f, 0);
        static Vector3 LEFTDOWN = new Vector3(-2.5f, -0.8f, 0);
        static Vector3 RIGHTDOWN = new Vector3(-1.5f, -0.8f, 0);
        static Vector3 BOATUP = new Vector3(-2, -0.4f, 0);
        static Vector3 BOATDOWN = new Vector3(-2, -1.4f, 0);
        static float SPEED = 4f;
    }
```

​

#### （二）自定义`MoveToAction`组件

​	自定义`MoveToAction`动作组件，其主要功能是包含一个实现让特定对象移动到特定地点的动作。需要注意的是，在工程中年我们常常先自定义一个动作基类，因为在游戏工程中往往需要各种各样的动作，我们将所有的动作定义为动作基类的子类，这符合封装的原则。`MoveToAction`需要实现两个主要方法，一个是`setMoveTo(Vector3 target, float speed)`，用于设置动作的目标位置与移动速度，另一个是重写`MonoBehaviour`的`Update()`方法，该方法会在游戏的每一帧执行，用以动作的具体实现和检查动作是否完成。

```c#
	public class MyAction : MonoBehaviour
    {
       //...
    }

    public class MoveToAction : MyAction
    {
        Vector3 _target;
        float _speed;
        ActionState _actionState = ActionState.ing；

        public void setMoveTo(Vector3 target, float speed)
        {
            //do
        }

        private void Update()
        {
            //do
        }
    }
```



#### （三）应用动作函数

​	在`ActionManager`中实现你所需要的各种动作的应用，下面是一个例子。场景控制类只需调用这些动作应用函数即可实现对象的动作：

```c#
	public void moveToLeftUp(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(LEFTUP, SPEED);
        }
```

------

### 写在后面

​	游戏的实现还是有很多由于作为新手而作出的不理智选择，有很多值得改进的地方。此文也有许多不严谨处，欢迎大家多加指正。