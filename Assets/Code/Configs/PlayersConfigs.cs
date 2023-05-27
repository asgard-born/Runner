using Code.Player;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayersConfigs", fileName = "PlayersConfigs")]
    public class PlayersConfigs : ScriptableObject
    {
        public PlayerEntity playerPrefab;
        public int cameraSmooth = 6;
        public int initialSpeed = 15;
    }
}