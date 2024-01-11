using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/ResourcesConfigs", fileName = "ResourcesConfigs")]
    public class ResourcesConfigs : ScriptableObject
    {
        public AssetReference characterReference;
        public AssetReference hudViewReference;
        public AssetReference winViewReference;
        public AssetReference looseViewReference;
        public AssetReference virtualPadReference;

    }
}