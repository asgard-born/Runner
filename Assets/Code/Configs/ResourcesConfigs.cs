using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/ResourcesConfigs", fileName = "ResourcesConfigs")]
    public class ResourcesConfigs : ScriptableObject
    {
        public string characterPath;
        public string hudViewPath;
        public string winViewPath;
        public string looseViewPath;
    }
}