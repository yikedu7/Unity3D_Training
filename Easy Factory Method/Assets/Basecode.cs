using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

namespace Com.MyGame
{
    public class EsayFactory : System.Object
    {
        public List<GameObject> markList = new List<GameObject>();
        private static EsayFactory _factory;
        private Camera _camera;

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
                    if (markList.Count == 0)
                    {
                        attackMark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        attackMark.GetComponent<Collider>().enabled = false;
                    }
                    else
                    {
                        attackMark = markList[0];
                        markList.RemoveAt(0);
                    }
                    attackMark.transform.SetParent(_hit.collider.gameObject.transform);
                    attackMark.transform.position = _hit.point;
                    DelayToDelete _delay = attackMark.AddComponent<DelayToDelete>();
                    _delay.delete(markList, attackMark, 2.0f);
                }
            }
        }

        public class DelayToDelete : MonoBehaviour
        {
            //private List<GameObject> markList;
            //private GameObject attackMark;
            public void delete(List<GameObject> markList, GameObject attackMark, float delaySeconds)
            {
                StartCoroutine(delayTo(() =>
                {
                    attackMark.transform.position = new Vector3(0f, -99f, 0f);
                    markList.Add(attackMark);
                }
                , 2.0f));
            }

            private IEnumerator delayTo(Action action, float delaySeconds)
            {
                yield return new WaitForSeconds(delaySeconds);
                action();
            }
        }
    }
}

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
