# README

### 游戏需求

* 实现点击效果。
* 用 `Plane` 或其他物体做地面， tag 为`Finish`。
* 点击地面后，出现一个圆形攻击标记，两秒后自动消失。注意：该攻击标记不能挡住点击。（Primitive Objects / Cylinder）
* 请使用一个简单工厂创建、管理这些的标记，并自动收回这些标记（注意，这些对象创建后，放在列表内，不必释放）。

### 效果展示：

![](http://ompnv884d.bkt.clouddn.com/Easy%20Factory%20Method.gif)



### 实现过程

#### （一）`Camera` 和 `Plane` 的基本配置

​	设置好`Camera`的位置与角度，并设置视图为正交，如下图：

![](http://ompnv884d.bkt.clouddn.com/EasyFactory1.JPG-default)

​	为方便显示，创建`Plane`后再添加一个`Material`，使其变为蓝色，如下图：

![](http://ompnv884d.bkt.clouddn.com/EasyFactory2.JPG-default)

#### （二）定义并实现`EsayFactory`类

​	首先创建一个继承`MonoBehaviour`的`Basecode`类，自定义一个`namespace`并引用，在该`namespace`定义并实现`EsayFactory`类。

​	`EsayFactory`类为工厂类，它负责管理`Cylinder`的创建、停用和重复使用。重复使用的实现主要通过讲使用完毕的`Cylinder`加入一个`List`中，在需要时重新拿出使用的方法来实现。这样做的好处是，避免了重复地创建和销毁游戏对象，能减少游戏的内存消耗。

​	需要注意的是：严格控制单实例，避免用户创建多个`EsayFactory`对象。

```c#
public class EsayFactory : System.Object
{
	public List<GameObject> markList = new List<GameObject>();
	private static EsayFactory _factory;
	private Camera _camera;
	
  	//控制单实例
	public static EsayFactory GetInstance()
	{
		if (_factory == null)
		{
			_factory = new EsayFactory();
			_factory._camera = Camera.main;
		}
		return _factory;
	}

	public void placeAttackMark(Vector3 target)
	{
		Ray _ray = _camera.ScreenPointToRay(target);
		RaycastHit _hit;
		if (Physics.Raycast(_ray, out _hit))
		{
			if (_hit.collider.gameObject.tag.Contains("Finish"))
			{
				GameObject attackMark;
              	//List中若无可用对象则创建新对象
				if (markList.Count == 0)
				{
					attackMark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                  	//避免Cylinder挡住点击
					attackMark.GetComponent<Collider>().enabled = false;
				}
              	//若有可用对象则拿出重用
				else
				{
					attackMark = markList[0];
					markList.RemoveAt(0);
				}
			attackMark.transform.SetParent(_hit.collider.gameObject.transform);
			attackMark.transform.position = _hit.point;
              //添加一个自定义的实现了延时2s删除功能的组件
			DelayToDelete _delay = attackMark.AddComponent<DelayToDelete>();
			_delay.delete(markList, attackMark, 2.0f);
		}
	}
}
```

#### （三）实现延时删除

​	`DelayToDelete`继承`MonoBehaviour`类，实现了两个方法：私有方法`delayTo()`实现了具体延时功能的实现，公有方法`delete()`实现了具体的2秒后删除的功能需求。这里采用了`Action`类来代替`Invoke()`方法，其好处是拓展性更强，可读性也更高。`StartCoroutine`是`MonoBehaviour`中的一个调用协同程序的方法，通过使用 `yield return new WaitForSeconds(delaySeconds);`可以实现延时一定实现后执行。

```c#
public class DelayToDelete : MonoBehaviour
{
    public void delete(List<GameObject> markList, GameObject attackMark, float 	delaySeconds)
    {
        StartCoroutine(delayTo(() =>
        {
          //移出游戏范围并加入List中等待重用
            attackMark.transform.position = new Vector3(0f, -99f, 0f);
            markList.Add(attackMark);
        }
        , 2.0f));
    }

    private IEnumerator delayTo(Action action, float delaySeconds)
    {
      //延时执行action()；
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
```

#### （四）完成`Basecode`

​	`Basecode`类主要负责每帧检查用户是否做了点击事件，若点击则调用`placeAttackMark`方法：

```c#
public class Basecode : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = Input.mousePosition;
            EsayFactory.GetInstance().placeAttackMark(point);
        }
	}
}
```




