using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.ObjectsPool
{
    public static class PoolsManager
    {
        private static Dictionary<Type, PoolInfo> _pools = new Dictionary<Type, PoolInfo>();
        private static GameObject _objectsParent;

        public static void Initialize(Dictionary<Type, PoolInfo> newPools)
        {
            _pools = newPools;
            _objectsParent = new GameObject();
            _objectsParent.name = "Platforms Pool";

            foreach (var pool in _pools.Values.Where(pool => pool.prefab != null))
            {
                pool.pool = new Pool();
                pool.pool.Initialize(pool.count, pool.prefab, _objectsParent.transform);
            }
        }

        public static T GetObject<T>() where T : PoolObject
        {
            T result = null;

            if (_pools.TryGetValue(typeof(T), out var poolInfo))
            {
                result = poolInfo.pool.GetObject<T>();
                result.gameObject.SetActive(true);
            }

            return result;
        }

        public static T GetObject<T>(Vector3 position, Quaternion rotation) where T : PoolObject
        {
            T result = null;

            if (_pools.TryGetValue(typeof(T), out var poolInfo))
            {
                result = poolInfo.pool.GetObject<T>();
                result.transform.position = position;
                result.transform.rotation = rotation;
                result.gameObject.SetActive(true);
            }

            return result;
        }
    }
}