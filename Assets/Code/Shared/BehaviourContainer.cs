using System;
using Character.Behaviour;
using UnityEngine;

namespace Shared
{
    /// <summary>
    /// Обертка для связки енама и типа поведения 
    /// </summary>
    [Serializable]
    public class BehaviourContainer<T> where T : CharacterBehaviour
    {
        [SerializeField] private BehaviourType _behaviourType;
        [SerializeField] private T _behaviour;

        public BehaviourType behaviourType => _behaviourType;
        public T behaviour => _behaviour;

        public BehaviourContainer(BehaviourType behaviourType, T behaviour)
        {
            if (behaviourType == BehaviourType.None)
            {
                Debug.LogError("The behaviour must be defined");

                return;
            }

            _behaviourType = behaviourType;
            _behaviour = behaviour;
        }
    }
}