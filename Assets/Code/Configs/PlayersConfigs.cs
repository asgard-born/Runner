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
        [SerializeField] private Vector2 _toleranceDistance;
        [SerializeField] private LayerMask _landingMask;

        public BehaviourInfo initialBehaviourInfo => _initialBehaviourInfo;
        public Vector2 toleranceDistance => _toleranceDistance;
        public LayerMask landingMask => _landingMask;

        private void OnValidate()
        {
            Assert.IsTrue(_initialBehaviourInfo != null, "Initial behaviour cannot be null");
            Assert.IsTrue(_initialBehaviourInfo.configs.type == BehaviourType.Moving, "Initial behaviour must have Moving type");
        }
    }
}