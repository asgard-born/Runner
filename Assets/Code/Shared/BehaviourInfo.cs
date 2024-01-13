using System;
using Configs;
using Sirenix.OdinInspector;
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
        [SerializeField] private bool _isEndless;
        [SerializeField, HideIf("_isEndless")] private float _durationSec;
        
        public bool isEndless => _isEndless;
        public float durationSec => _durationSec;
        public BehaviourConfigs configs => _configs;
    }
}