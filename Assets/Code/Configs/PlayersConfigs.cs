using Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace Configs
{
    /// <summary>
    /// Начальные установки игрока, включая его начальное поведение
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Players Configs", fileName = "Players_Configs")]
    public class PlayersConfigs : ScriptableObject
    {
        [SerializeField] private BehaviourInfo _initialBehaviourInfo;
        [SerializeField] private float _initialSpeed = 15f;
        [SerializeField] private float _jumpForce = 4f;

        public BehaviourInfo initialBehaviourInfo => _initialBehaviourInfo;
        public float initialSpeed => _initialSpeed;
        public float jumpForce => _jumpForce;

        private void OnValidate()
        {
            Assert.IsTrue(_initialBehaviourInfo != null, "Initial behaviour cannot be null");
            Assert.IsTrue(_initialBehaviourInfo.configs.type == BehaviourType.Moving, "Initial behaviour must have Moving type");
        }
    }
}