using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    /// <summary>
    /// Конфиги для загружаемых ресурсов
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Resources Configs", fileName = "Resources_Configs")]
    public class ResourcesConfigs : ScriptableObject
    {
        public AssetReference characterReference;
        public AssetReference hudViewReference;
        public AssetReference winViewReference;
        public AssetReference looseViewReference;
        public AssetReference virtualPadReference;

    }
}