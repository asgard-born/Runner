using System;
using Configs;
using UnityEngine;

namespace Shared
{
    /// <summary>
    /// Служит для полной компоновки всех настроек поведения: конфигов и времени действия,
    /// которые разделены для гибкости изменения
    /// </summary>
    [Serializable]
    public class BehaviourInfo
    {
        [SerializeField] private BehaviourConfigs _configs;
        [SerializeField] private float _durationSec;
        
        public float durationSec => _durationSec;
        public BehaviourConfigs configs => _configs;
    }
}