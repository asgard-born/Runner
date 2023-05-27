using System.Collections.Generic;
using System.Linq;
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
            var platformToSpawn = CalculatePlatformToSpawn();

            if (platformToSpawn == null)
            {
                Debug.LogError("Cannot spawn platform, check the Platform's chances in Current level's configs");
            }

            var type = platformToSpawn.GetType();

            var newPlatform = (Platform)PoolsManager.GetObject(type);

            Vector3 newPosition = parent.position;

            if (lastPlatform != null)
            {
                newPosition = lastPlatform.transform.position + Vector3.forward * (lastPlatform.transform.lossyScale.z / 2 + newPlatform.transform.lossyScale.z / 2);
            }

            newPlatform.transform.position = newPosition;
            newPlatform.transform.parent = parent;

            _spawnCount++;

            _platforms.AddLast(newPlatform);

            return newPlatform;
        }

        private Platform CalculatePlatformToSpawn()
        {
            var chances = _levelConfigs.platformChances.OrderBy(x => x.chance);
            var summ = chances.Sum(x => x.chance);

            var randomNumber = Random.Range(0, summ);

            var currentSumm = 0f;

            foreach (var platformChance in chances)
            {
                currentSumm += platformChance.chance;

                if (randomNumber <= currentSumm)
                {
                    return platformChance.platform;
                }
            }

            return null;
        }
    }
}