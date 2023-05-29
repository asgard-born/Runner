using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Configs;
using Code.Configs.Essences;
using Code.ObjectsPool;
using Code.Platforms.Abstract;
using Code.Platforms.Concrete;
using Code.Platforms.Essences;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Platforms
{
    public class PlatformsSpawningSystem
    {
        private LevelConfigs _levelConfigs;
        private int _spawnCount;
        private readonly LinkedList<Platform> _platforms = new LinkedList<Platform>();
        private const int _leftYAngle = 270;
        private Platform _lastPlatform;

        public PlatformsSpawningSystem(LevelConfigs levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public Platform SpawnImmediately(Transform parent, Action<Type> interactionCallback)
        {
            for (var i = 0; i < _levelConfigs.firstGuaranteedPlatformsCount; i++)
            {
                _lastPlatform = CreatePlatformWithRestrictions(parent, interactionCallback, _levelConfigs.firstGuaranteedPlatforms, true);
            }

            var countToInitialSpawn = _levelConfigs.startPlatformsCount - _levelConfigs.firstGuaranteedPlatformsCount;

            for (var i = 0; i < countToInitialSpawn; i++)
            {
                var cannotDublicatePlatforms = _levelConfigs.cannotDublicatePlatforms;
                
                if (cannotDublicatePlatforms.Contains(_lastPlatform.platformType))
                {
                    _lastPlatform = CreatePlatformWithRestrictions(parent, interactionCallback, cannotDublicatePlatforms, false);
                }
                else
                {
                    _lastPlatform = CreatePlatform(parent, interactionCallback);
                }
            }

            return _platforms.First.Value;
        }

        public async void StartSpawningCycleAsync(Transform parent, Action<Type> interactionCallback)
        {
            while (_spawnCount < _levelConfigs.allPlatformsCount)
            {
                if (_platforms.Count >= _levelConfigs.maxPlatformsInTime)
                {
                    var firstPlatform = _platforms.First.Value;
                    firstPlatform.ReturnToPool();
                    firstPlatform.Dispose();
                    _platforms.RemoveFirst();
                }

                var cannotDublicatePlatforms = _levelConfigs.cannotDublicatePlatforms;
                
                if (cannotDublicatePlatforms.Contains(_lastPlatform.platformType))
                {
                    _lastPlatform = CreatePlatformWithRestrictions(parent, interactionCallback, cannotDublicatePlatforms, false);
                }
                else
                {
                    _lastPlatform = CreatePlatform(parent, interactionCallback);
                }

                await Task.Delay((int)(_levelConfigs.spawnPlatformsDelaySec * 1000));
            }
        }

        private Platform CreatePlatformWithRestrictions(Transform parent, Action<Type> interactionCallback, HashSet<PlatformType> types, bool isAvailable)
        {
            PlatformChance[] platformChances = FilterPlatformsToSpawnByTypes(types, isAvailable);
            Platform platformToSpawn = CalculatePlatformToSpawn(platformChances);

            if (platformToSpawn == null)
            {
                Debug.LogError("Cannot spawn platform, check the Platform's chances in Current level's configs");
            }

            return Spawn(platformToSpawn, parent, interactionCallback);
        }

        private Platform CreatePlatform(Transform parent, Action<Type> interactionCallback)
        {
            Platform platformToSpawn = CalculatePlatformToSpawn(_levelConfigs.platformChances);

            if (platformToSpawn == null)
            {
                Debug.LogError("Cannot spawn platform, check the Platform's chances in Current level's configs");
            }

            return Spawn(platformToSpawn, parent, interactionCallback);
        }
        
        private Platform Spawn(Platform platformToSpawn, Transform parent, Action<Type> interactionCallback)
        {
            var type = platformToSpawn.GetType();

            var nextPlatform = (Platform)PoolsManager.GetObject(type);

            Vector3 nextPosition = parent.position;

            if (_lastPlatform != null)
            {
                nextPosition = CalculatePositionForPlatform(_lastPlatform, nextPlatform);

                nextPlatform.transform.rotation = _lastPlatform.transform.rotation;

                if (_lastPlatform is TurnPlatform previousTurnedPlatform)
                {
                    CalculateRotationForPlatform(previousTurnedPlatform, nextPlatform);
                }
            }

            nextPlatform.transform.position = nextPosition;
            nextPlatform.transform.parent = parent;
            nextPlatform.OnInterractedWithPlayer += interactionCallback;

            _spawnCount++;

            _platforms.AddLast(nextPlatform);

            return nextPlatform;
        }

        private void CalculateRotationForPlatform(TurnPlatform previousTurnedPlatform, Platform nextPlatform)
        {
            if (previousTurnedPlatform is TurnLeftPlatform)
            {
                nextPlatform.transform.Rotate(0, _leftYAngle, 0);
            }
            else if (previousTurnedPlatform is TurnRightPlatform)
            {
                nextPlatform.transform.Rotate(0, -_leftYAngle, 0);
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

        private Platform CalculatePlatformToSpawn(PlatformChance[] platformChances)
        {
            var chances = platformChances.OrderBy(platformChance => platformChance.chance);

            var sum = chances.Sum(x => x.chance);

            var randomNumber = Random.Range(0, sum);

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

        private PlatformChance[] FilterPlatformsToSpawnByTypes(HashSet<PlatformType> types, bool isAvailable = true)
        {
            if (types != null && types.Count > 0)
            {
                var platformChances = _levelConfigs.platformChances.Where(p => isAvailable ? types.Contains(p.platform.platformType) : !types.Contains(p.platform.platformType)).ToArray();

                return platformChances;
            }

            return _levelConfigs.platformChances.ToArray();
        }
    }
}