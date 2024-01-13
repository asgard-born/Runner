using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Конфиги для установки каждого из уровней
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Level Configs", fileName = "Level_Configs")]
    public class LevelConfigs : ScriptableObject
    {
        [Space, Header("Start")] private float _startDelaySec = 1.2f;
        public float startDelaySec => _startDelaySec;
    }
}