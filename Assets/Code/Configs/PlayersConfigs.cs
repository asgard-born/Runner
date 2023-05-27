using Code.Player;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayersConfigs", fileName = "PlayersConfigs")]
    public class PlayersConfigs : ScriptableObject
    {
        public PlayerEntity playerPrefab;
        public float speed;
    }
}