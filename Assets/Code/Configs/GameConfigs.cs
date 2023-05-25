using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameConfigs", fileName = "GameConfigs", order = 0)]
    public class GameConfigs : ScriptableObject
    {
        public int maxPlatformsCount;
    }
}