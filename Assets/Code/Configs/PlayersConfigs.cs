using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayersConfigs", fileName = "PlayersConfigs")]
    public class PlayersConfigs : ScriptableObject
    {
        public float initialSpeed = 15f;
        public float jumpForce = 4f;
    }
}