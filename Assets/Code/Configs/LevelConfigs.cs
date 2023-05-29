using System.Collections.Generic;
using System.Linq;
using Code.Configs.Essences;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        public PlatformType[] firstGuaranteedPlatformTypes;
        public PlatformType[] cannotDublicatePlatformTypes;
        public int firstGuaranteedPlatformsCount = 5;
        public int startPlatformsCount = 50;
        public float spawnPlatformsDelaySec = .8f;
        public float runDelaySec = 1.2f;
        public int allPlatformsCount = 400;
        public int maxPlatformsInTime = 100;
        public PlatformChance[] platformChances;

        public HashSet<PlatformType> firstGuaranteedPlatforms;
        public HashSet<PlatformType> cannotDublicatePlatforms;

        private HashSet<PlatformType> hashSetTypes;

        private void OnValidate()
        {
            CheckForErrorsInCount();
            CalculateTotalChance();

            firstGuaranteedPlatforms = MakePlatformListUnique(firstGuaranteedPlatformTypes);
            cannotDublicatePlatforms = MakePlatformListUnique(cannotDublicatePlatformTypes);
        }

        private HashSet<PlatformType> MakePlatformListUnique(IList<PlatformType> listToCheck)
        {
            var filteredGuaranteedPlatformTypes = listToCheck.Where(x => x != PlatformType.None);

            return new HashSet<PlatformType>(filteredGuaranteedPlatformTypes.Distinct());
        }

        private void CheckForErrorsInCount()
        {
            if (firstGuaranteedPlatformsCount > startPlatformsCount)
            {
                var firstCount = firstGuaranteedPlatformsCount;
                firstGuaranteedPlatformsCount = startPlatformsCount;
                startPlatformsCount = firstCount;
            }
        }

        private void CalculateTotalChance()
        {
            var totalChance = 0f;

            foreach (var platformChance in platformChances)
            {
                totalChance += platformChance.chance;
            }

            Debug.Log($"The summ of chances is {totalChance}");
        }
    }
}