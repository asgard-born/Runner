using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.ObjectsPool
{
    public class Pool
    {
        private LinkedList<PoolObject> _poolObjects { get; set; }
        private Transform _objectsParent;

        public void Initialize<T>(int count, T sample, Transform parent) where T : PoolObject
        {
            _poolObjects = new LinkedList<PoolObject>();

            _objectsParent = parent;

            for (var i = 0; i < count; i++)
            {
                AddObject(sample, parent);
            }
        }

        public PoolObject GetObject()
        {
            foreach (var poolObject in _poolObjects)
            {
                if (!poolObject.gameObject.activeInHierarchy)
                {
                    return poolObject;
                }
            }

            AddObject(_poolObjects.First.Value, _objectsParent);
            _poolObjects.RemoveFirst();

            return _poolObjects.Last.Value;
        }

        private void AddObject(PoolObject sample, Transform objectsParent)
        {
            PoolObject poolObject;

            if (sample.gameObject.scene.name == null)
            {
                poolObject = Object.Instantiate(sample.gameObject, objectsParent, true).GetComponent<PoolObject>();
                poolObject.name = sample.name;
                poolObject.poolParent = _objectsParent;
            }
            else
            {
                poolObject = sample;
            }

            _poolObjects.AddLast(poolObject);
            poolObject.gameObject.SetActive(false);
        }
    }
}