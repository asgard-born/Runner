using System.Collections.Generic;
using System.Linq;
using Code.Configs.Essences;
using Code.Platforms.Concrete;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        [Header("Platforms")]
        [SerializeField] private PlatformType[] firstGuaranteedPlatformTypes;
        [SerializeField] private PlatformType[] cannotDublicatePlatformTypes;
        [SerializeField] private PlatformType[] platformTypesToCalculateOnFinish;
        
        public int firstGuaranteedPlatformsCount = 3;
        public int startPlatformsCount = 5;
        public float spawnPlatformsDelaySec = .4f;
        public float destroyPlatformsDelaySec = 1.5f;
        public int allPlatformsCount = 25;
        public int maxPlatformsInTime = 10;
        public FinishPlatform finishPlatform;
        public PlatformChance[] platformChances;
        
        [Space, Header("Start")]
        public float runDelaySec = 1.2f;
        
        [Space, Header("Bonuses")]
        public BonusChance[] bonusChances;

        public HashSet<PlatformType> firstGuaranteedPlatforms;
        public HashSet<PlatformType> cannotDublicatePlatforms;
        public HashSet<PlatformType> blocksToCalculateOnFinish;

        private HashSet<PlatformType> hashSetTypes;

        private void OnValidate()
        {
            CheckForErrorsInCount();
            CalculateTotalChance();

            firstGuaranteedPlatforms = MakePlatformArrayUnique(firstGuaranteedPlatformTypes);
            cannotDublicatePlatforms = MakePlatformArrayUnique(cannotDublicatePlatformTypes);
            blocksToCalculateOnFinish = MakePlatformArrayUnique(platformTypesToCalculateOnFinish);
        }

        private HashSet<PlatformType> MakePlatformArrayUnique(IList<PlatformType> listToCheck)
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