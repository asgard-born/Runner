using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Configs;
using Code.ObjectsPool;
using Code.Platforms.Abstract;
using Code.Platforms.Concrete;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsSpawningSystem
    {
        private LevelConfigs _levelConfigs;
        private int _spawnCount;
        private readonly LinkedList<Platform> _platforms = new LinkedList<Platform>();

        public PlatformsSpawningSystem(LevelConfigs levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public async void StartSpawning(Transform parent)
        {
            Platform lastPlatform = null;

            for (var i = 0; i < _levelConfigs.startPlatformsCount; i++)
            {
                lastPlatform = CreatePlatform(parent, lastPlatform);
            }

            while (_spawnCount < _levelConfigs.allPlatformsCount)
            {
                if (_platforms.Count >= _levelConfigs.maxPlatformsInTime)
                {
                    var firstPlatform = _platforms.First.Value;
                    firstPlatform.ReturnToPool();
                    _platforms.RemoveFirst();
                }

                lastPlatform = CreatePlatform(parent, lastPlatform);

                await Task.Delay((int)(_levelConfigs.spawnDelaySec * 1000));
            }
        }

        private Platform CreatePlatform(Transform parent, Platform lastPlatform)
        {
            var platform = PoolsManager.GetObject<StandardPlatform>();

            Vector3 newPosition = parent.position;

            if (lastPlatform != null)
            {
                newPosition = lastPlatform.transform.TransformPoint(Vector3.forward * platform.transform.lossyScale.z / 2);
            }

            platform.transform.position = newPosition;
            platform.transform.parent = parent;
            
            _spawnCount++;

            _platforms.AddLast(platform);

            return platform;
        }
    }
}