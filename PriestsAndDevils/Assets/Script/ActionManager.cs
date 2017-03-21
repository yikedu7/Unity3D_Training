using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;

namespace Com.Action
{
    public enum ActionState
    {
        no = 0,
        ing = 1
    }

    public class ActionManager : System.Object
    {
        private static ActionManager _IActionManager;
        static Vector3 LEFT = new Vector3(-0.4f, 0.4f, 0);
        static Vector3 RIGHT = new Vector3(0.4f, 0.4f, 0);
        static float SPEED = 5f;

        public static ActionManager GetInstance()
        {
            if (_IActionManager == null)
            {
                _IActionManager = new ActionManager();
            }
            return _IActionManager;
        }

        public void moveToLeft(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            //while (action._actionState == ActionState.ing)
                action.setMoveTo(LEFT, SPEED);
        }

        public void moveToRight(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
                action.setMoveTo(RIGHT, SPEED);
        }

        public void moveToStart(GameObject obj)
        {
            Vector3 start = obj.GetComponent<Person>().start;
            MoveToAction action = obj.AddComponent<MoveToAction>();
                action.setMoveTo(start, SPEED);
        }

        public void moveToArrive(GameObject obj)
        {
            Vector3 arrive = obj.GetComponent<Person>().arrive;
            MoveToAction action = obj.AddComponent<MoveToAction>();
                action.setMoveTo(arrive, SPEED);
        }
    }

    public class MyAction : MonoBehaviour
    {
        protected void free()
        {
            Destroy(this);
        }
    }

    public class MoveToAction : MyAction
    {
        Vector3 _target;
        float _speed;
        public ActionState _actionState { get; private set; }

        private MoveToAction _IMoveToAction = new MoveToAction();

        public void setMoveTo(Vector3 target, float speed)
        {
            _target = target;
            _speed = speed;
            _actionState = ActionState.ing;
        }

        private void Update()
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target, step);

            if (transform.localPosition == _target)
            {
                if (_actionState == ActionState.ing)
                {
                    _actionState = ActionState.no;
                }
                free();
            }
        }
    }
}



