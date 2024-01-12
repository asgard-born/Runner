using System;
using Character.Conditions;
using UnityEngine;

namespace Shared.Containers
{
    /// <summary>
    /// Обертка для связки енама с типом состояния
    /// </summary>
    [Serializable]
    public class ConditionContainer
    {
        private ConditionName _name;
        private Type _type;
        private EffectContainer[] _effects;
        private float _timeSec;

        public ConditionName name => _name;
        public Type type => _type;
        public EffectContainer[] effects => _effects;
        public float timeSec => _timeSec;

        public ConditionContainer(ConditionName conditionName, Type conditionType, EffectContainer[] effects, float timeSec)
        {
            if (conditionName == ConditionName.None)
            {
                Debug.LogError("The condition must be defined");

                return;
            }

            if (!conditionType.IsSubclassOf(typeof(CharacterCondition)))
            {
                Debug.LogError($"The condition must be inherited from {nameof(CharacterCondition)}");

                return;
            }

            if (_timeSec <= 0)
            {
                Debug.LogError($"The time of condition {conditionName.ToString()} must be greater than zero");

                return;
            }

            _effects = effects;
            _timeSec = timeSec;
            _name = conditionName;
            _type = conditionType;
        }
    }
}