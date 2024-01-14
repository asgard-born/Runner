﻿using Shared;
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
        [SerializeField] private float _toleranceSideDistance = .05f;

        public BehaviourInfo initialBehaviourInfo => _initialBehaviourInfo;
        public float toleranceSideDistance => _toleranceSideDistance;

        private void OnValidate()
        {
            Assert.IsTrue(_initialBehaviourInfo != null, "Initial behaviour cannot be null");
            Assert.IsTrue(_initialBehaviourInfo.configs.type == BehaviourType.Moving, "Initial behaviour must have Moving type");
        }
    }
}