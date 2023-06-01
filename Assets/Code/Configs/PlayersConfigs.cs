using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayersConfigs", fileName = "PlayersConfigs")]
    public class PlayersConfigs : ScriptableObject
    {
        public float cameraSmooth = 1.5f;
        public int initialSpeed = 15;
        public float jumpForce = 4;
        public int maxJumpingTimes = 2;
        public int lives = 5;
    }
}