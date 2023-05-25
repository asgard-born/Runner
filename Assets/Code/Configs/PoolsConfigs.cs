using System;
using System.Collections.Generic;
using System.Linq;
using Code.ObjectsPool;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PoolsConfigs", fileName = "PoolsConfigs", order = 0)]
    public class PoolsConfigs : ScriptableObject
    {
        public PoolInfo[] pools;
        public Dictionary<Type, PoolInfo> poolsDictionary = new Dictionary<Type, PoolInfo>();

        private void OnValidate()
        {
            poolsDictionary = pools.ToDictionary(p => p.prefab.GetType());
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            PoolsManager.Initialize(poolsDictionary);
        }
    }
}