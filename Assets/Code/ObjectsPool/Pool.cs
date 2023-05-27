using System.Collections.Generic;
using UnityEngine;

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

        public T GetObject<T>() where T : PoolObject
        {
            foreach (var poolObject in _poolObjects)
            {
                if (!poolObject.gameObject.activeInHierarchy)
                {
                    return (T)poolObject;
                }
            }

            AddObject(_poolObjects.First.Value, _objectsParent);
            _poolObjects.RemoveFirst();

            return (T)_poolObjects.Last.Value;
        }

        private void AddObject<T>(T sample, Transform objectsParent) where T : PoolObject
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

            _poolObjects.AddLast(poolObject.GetComponent<T>());
            poolObject.gameObject.SetActive(false);
        }
    }
}