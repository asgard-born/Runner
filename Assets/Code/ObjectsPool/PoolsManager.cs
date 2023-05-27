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

        public static PoolObject GetObject(Type type)
        {
            PoolObject result = null;

            if (_pools.TryGetValue(type, out var poolInfo))
            {
                result = poolInfo.pool.GetObject();
                result.gameObject.SetActive(true);
            }

            return result;
        }

        public static PoolObject GetObject(Type type, Vector3 position, Quaternion rotation)
        {
            PoolObject result = null;

            if (_pools.TryGetValue(type, out var poolInfo))
            {
                result = poolInfo.pool.GetObject();
                result.transform.position = position;
                result.transform.rotation = rotation;
                result.gameObject.SetActive(true);
            }

            return result;
        }
    }
}