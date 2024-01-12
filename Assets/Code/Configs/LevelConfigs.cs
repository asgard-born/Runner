using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        [Space, Header("Start")] public float startDelaySec = 1.2f;
    }
}