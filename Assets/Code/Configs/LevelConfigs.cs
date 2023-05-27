using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        public int startPlatformsCount = 5;
        public float spawnDelaySec = 1.5f;
        public int allPlatformsCount;
        public int maxPlatformsInTime;
    }
}