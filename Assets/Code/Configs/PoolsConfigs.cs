using System;
using System.Collections.Generic;
using System.Linq;
using Code.ObjectsPool;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PoolsConfigs", fileName = "PoolsConfigs")]
    public class PoolsConfigs : ScriptableObject
    {
        public PoolInfo[] pools;
        public Dictionary<Type, PoolInfo> poolsDictionary;

        private void OnValidate()
        {
            poolsDictionary = pools.ToDictionary(p => p.prefab.GetType());
        }

        public void Initialize()
        {
            PoolsManager.Initialize(poolsDictionary);
        }
    }
}