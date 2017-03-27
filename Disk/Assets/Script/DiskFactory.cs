using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;

namespace Com.Manager
{
    public class DiskFactory
    {
        public GameObject diskPrefab;

        private static DiskFactory _diskFactory;
        List<GameObject> usingDisks = new List<GameObject>();
        List<GameObject> uselessDisks = new List<GameObject>();

        public static DiskFactory getFactory()
        {
            if (_diskFactory == null)
                _diskFactory = new DiskFactory();
            return _diskFactory;
        }

        public List<GameObject> prepareDisks (int diskCount)
        {
            for (int i = 0; i < diskCount; i++)
            {
                if (uselessDisks.Count == 0)
                {
                    uselessDisks[0] = GameObject.Instantiate<GameObject>(diskPrefab);
                    uselessDisks[0].AddComponent<Renderer>();
                }
                else
                {
                    usingDisks[i] = uselessDisks[0];
                    uselessDisks.RemoveAt(0);
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
