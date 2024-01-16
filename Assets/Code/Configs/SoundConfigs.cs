using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Отведенная область для хранения звуков и музыки. Список звуков, которые будут
    /// добавляться, можно держать в словаре по отдельным ключам enum и запускать по этим же ключам
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Audio Configs", fileName = "Audio_Configs")]
    public class AudioConfigs : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _musicList;
        
        public List<AudioClip> musicList => _musicList;
    }
}