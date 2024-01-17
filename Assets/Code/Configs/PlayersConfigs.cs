using Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace Configs
{
    /// <summary>
    /// Начальные установки игрока, включая его стартовое поведение
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Players Configs", fileName = "Players_Configs")]
    public class PlayersConfigs : ScriptableObject
    {
        [SerializeField] private BehaviourInfo _initialBehaviourInfo;
        [SerializeField] private Vector2 _toleranceDistance;
        [SerializeField] private LayerMask _landingMask;
        [SerializeField] private float _crashDelay;
        [SerializeField] private Vector3 _initialSpeed;
        [SerializeField] private int _initialLives;

        public BehaviourInfo initialBehaviourInfo => _initialBehaviourInfo;
        public Vector2 toleranceDistance => _toleranceDistance;
        public float crashDelay => _crashDelay;
        public LayerMask landingMask => _landingMask;
        public Vector3 initialSpeed => _initialSpeed;
        public int initialLives => _initialLives;

        private void OnValidate()
        {
            Assert.IsTrue(_initialBehaviourInfo != null, "Initial behaviour cannot be null");
            Assert.IsTrue(_initialBehaviourInfo.configs.key == BehaviourKey.Moving, "Initial behaviour must have Moving key");
        }
    }
}