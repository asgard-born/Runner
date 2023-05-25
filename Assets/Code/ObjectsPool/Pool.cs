using System.Collections.Generic;
using UnityEngine;

namespace Code.ObjectsPool
{
    public class Pool
    {
        private List<PoolObject> PoolObjects { get; set; }
        Transform objectsParent;

        public void Initialize(int count, PoolObject sample, Transform parent)
        {
            PoolObjects = new List<PoolObject>();
            
            objectsParent = parent;

            for (var i = 0; i < count; i++)
            {
                AddObject(sample, parent);
            }
        }

        public void AddObject(PoolObject sample, Transform objectsParent)
        {
            GameObject temp;

            if (sample.gameObject.scene.name == null)
            {
                temp = Object.Instantiate(sample.gameObject, objectsParent, true);
                temp.name = sample.name;
            }
            else
            {
                temp = sample.gameObject;
            }

            PoolObjects.Add(temp.GetComponent<PoolObject>());
            temp.SetActive(false);
        }

        public PoolObject GetObject()
        {
            for (var i = 0; i < PoolObjects.Count; i++)
            {
                if (PoolObjects[i].gameObject.activeInHierarchy == false)
                    return PoolObjects[i];
            }

            AddObject(PoolObjects[0], objectsParent);
            PoolObjects.RemoveAt(0);

            return PoolObjects[PoolObjects.Count - 1];
        }
    }
}