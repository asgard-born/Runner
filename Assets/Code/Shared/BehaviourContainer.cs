using System;
using Character.Behaviour;
using UnityEngine;

namespace Shared
{
    /// <summary>
    /// Обертка для связки енама с типом поведения
    /// Подходит для передачи в события
    /// </summary>
    [Serializable]
    public class BehaviourContainer
    {
        private Type _type;
        private EffectContainer[] _effects;
        private float _timeSec;

        public Type type => _type;
        public EffectContainer[] effects => _effects;
        public float timeSec => _timeSec;

        public BehaviourContainer(Type behaviourType, EffectContainer[] effects, float timeSec)
        {
            if (!behaviourType.IsSubclassOf(typeof(CharacterBehaviour)))
            {
                Debug.LogError($"The behaviour must be inherited from {nameof(CharacterBehaviour)}");

                return;
            }

            if (_timeSec <= 0)
            {
                Debug.LogError($"The time of behaviour {behaviourType.Name} must be greater than zero");

                return;
            }

            _effects = effects;
            _timeSec = timeSec;
            _type = behaviourType;
        }
    }
}