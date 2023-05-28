using Code.Player;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayersConfigs", fileName = "PlayersConfigs")]
    public class PlayersConfigs : ScriptableObject
    {
        public PlayerEntity playerPrefab;
        public float cameraSmooth = 1.5f;
        public int initialSpeed = 15;
        public float jumpForce = 4;
    }
}