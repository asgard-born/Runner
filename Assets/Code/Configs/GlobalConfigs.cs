using System.Collections.Generic;
using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Глобальные конфиги
    /// Содержит словарь с мапингом слоев на их имена. Таким образом мы отходим от проверки столкновений по типу
    /// для тех объектов, где отсутствует необходимость наличия компонентов MonoBehaviour
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Global Configs", fileName = "Global_Configs")]
    public class GlobalConfigs : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<LayerMask, LayerName> _layersDictionary;
        
        public Dictionary<LayerMask, LayerName> layersDictionary => _layersDictionary;
    }
}