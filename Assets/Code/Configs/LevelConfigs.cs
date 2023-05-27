using System.Collections.Generic;
using Code.Configs.Essences;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        public int startPlatformsCount = 5;
        public float spawnPlatformsDelaySec = .4f;
        public float runDelaySec = 1.2f;
        public int allPlatformsCount;
        public int maxPlatformsInTime;
        public List<PlatformChance> platformChances;

        private void OnValidate()
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