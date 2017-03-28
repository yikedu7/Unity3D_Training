using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

namespace Com.Manager
{
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
}

public class DiskFactoryMono : MonoBehaviour
{
    DiskFactory _diskFactory;
    public GameObject diskPrefab;

    void Awake()
    {
        _diskFactory = DiskFactory.getFactory();
        _diskFactory.diskPrefab = this.diskPrefab;
    }
}
