using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Начальные установки игрока
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Players Configs", fileName = "Players_Configs")]
    public class PlayersConfigs : ScriptableObject
    {
        [SerializeField] private float _initialSpeed = 15f;
        [SerializeField] private float _jumpForce = 4f;

        public float initialSpeed => _initialSpeed;
        public float jumpForce => _jumpForce;
    }
}