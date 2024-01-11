using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public class LevelConfigs : ScriptableObject
    {
        [Space, Header("Start")] public float startDelaySec = 1.2f;

        public Vector3 cameraPositionOffset = new Vector3(0, 4, -6);
        public Vector3 cameraRotationOffset = new Vector3(15, 0, 0);
    }
}