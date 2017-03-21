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
        static Vector3 LEFTUP = new Vector3(-2.5f, 0.2f, 0);
        static Vector3 RIGHTUP = new Vector3(-1.5f, 0.2f, 0);
        static Vector3 LEFTDOWN = new Vector3(-2.5f, -0.8f, 0);
        static Vector3 RIGHTDOWN = new Vector3(-1.5f, -0.8f, 0);
        static Vector3 BOATUP = new Vector3(-2, -0.4f, 0);
        static Vector3 BOATDOWN = new Vector3(-2, -1.4f, 0);
        static float SPEED = 4f;

        public static ActionManager GetInstance()
        {
            if (_IActionManager == null)
            {
                _IActionManager = new ActionManager();
            }
            return _IActionManager;
        }

        public void moveToLeftUp(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(LEFTUP, SPEED);
        }

        public void moveToRightUp(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(RIGHTUP, SPEED);
        }

        public void moveToLeftDown(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(LEFTDOWN, SPEED);
        }

        public void moveToRightDown(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(RIGHTDOWN, SPEED);
        }

        public void moveToStart(GameObject obj)
        {
            Vector3 start = obj.GetComponent<Person>().start;
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(start, SPEED);
        }

        public void moveToArrive(GameObject obj)
        {
            Vector3 arrive = obj.GetComponent<Person>().arrive;
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(arrive, SPEED);
        }

        public void boatMoveDown(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(BOATDOWN, SPEED);
        }

        public void boatMoveUp(GameObject obj)
        {
            MoveToAction action = obj.AddComponent<MoveToAction>();
            if (action.actionState == ActionState.ing)
                action.setMoveTo(BOATUP, SPEED);
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
        ActionState _actionState = ActionState.ing;
        public ActionState actionState
        {
            get { return _actionState; }
            private set { _actionState = value; }
        }


        public void setMoveTo(Vector3 target, float speed)
        {
            _target = target;
            _speed = speed;
        }

        private void Update()
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target, step);

            if (transform.position == _target)
            {
                if (actionState == ActionState.ing)
                {
                    actionState = ActionState.no;
                }
                Destroy(this);
            }
        }
    }
}



