using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Configs;
using Code.ObjectsPool;
using Code.Platforms.Abstract;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsSpawningSystem
    {
        private LevelConfigs _levelConfigs;
        private int _spawnCount;
        private readonly LinkedList<Platform> _platforms = new LinkedList<Platform>();
        private const int _leftYAngle = 270;

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

                await Task.Delay((int)(_levelConfigs.spawnPlatformsDelaySec * 1000));
            }
        }

        private Platform CreatePlatform(Transform parent, Platform previousPlatform)
        {
            var platformToSpawn = CalculatePlatformToSpawn();

            if (platformToSpawn == null)
            {
                Debug.LogError("Cannot spawn platform, check the Platform's chances in Current level's configs");
            }

            var type = platformToSpawn.GetType();

            var nextPlatform = (Platform)PoolsManager.GetObject(type);

            Vector3 nextPosition = parent.position;

            if (previousPlatform != null)
            {
                nextPosition = CalculatePositionForPlatform(previousPlatform, nextPlatform);

                nextPlatform.transform.rotation = previousPlatform.transform.rotation;

                if (previousPlatform is TurnPlatform previousTurnedPlatform)
                {
                    CalculateRotationForPlatform(previousTurnedPlatform, nextPlatform);
                }
            }

            nextPlatform.transform.position = nextPosition;
            nextPlatform.transform.parent = parent;

            _spawnCount++;

            _platforms.AddLast(nextPlatform);

            return nextPlatform;
        }

        private void CalculateRotationForPlatform(TurnPlatform previousTurnedPlatform, Platform nextPlatform)
        {
            switch (previousTurnedPlatform.platformType)
            {
                case PlatformType.TurnLeft:
                    nextPlatform.transform.Rotate(0, _leftYAngle, 0);

                    break;

                case PlatformType.TurnRight:
                    nextPlatform.transform.Rotate(0, -_leftYAngle, 0);

                    break;
            }
        }

        private Vector3 CalculatePositionForPlatform(Platform previousPlatform, Platform nextPlatform)
        {
            Vector3 position;

            if (previousPlatform is TurnPlatform lastTurnPlatform)
            {
                position = lastTurnPlatform.lastPartTransform.position +
                           lastTurnPlatform.lastPartTransform.forward.normalized * (lastTurnPlatform.lastPartTransform.lossyScale.z / 2 + nextPlatform.transform.lossyScale.z / 2);
            }
            else
            {
                var prevTransform = previousPlatform.transform;
                position = prevTransform.position + prevTransform.forward * (prevTransform.lossyScale.z / 2 + nextPlatform.transform.lossyScale.z / 2);
            }

            return position;
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