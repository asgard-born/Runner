using System.Collections.Generic;
using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Конфиги с привязкой к определенному уровню. Содержит словарь с мапингом слоев на их имена.
    /// Таким образом мы отходим от проверки столкновений по типу наследников MonoBehaviour, что позволяет уменьшить их количество
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Global Configs", fileName = "Global_Configs")]
    public class LevelConfigs : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<LayerMask, LayerName> _layersDictionary;
        [SerializeField] private float _startDelaySec = 3f;

        public Dictionary<LayerMask, LayerName> layersDictionary => _layersDictionary;
        public float startDelaySec => _startDelaySec;
    }
}