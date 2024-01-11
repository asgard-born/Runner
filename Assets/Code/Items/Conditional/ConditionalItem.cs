using System;
using Character.Conditions;
using Shared;
using UnityEngine;
using UnityEngine.Assertions;

namespace Items.Conditional
{
    public abstract class ConditionalItem : Item
    {
        [SerializeField] protected ConditionName _conditionName;
        
        protected ConditionContainer _conditionContainer;

        public ConditionContainer conditionContainer => _conditionContainer;

        private void OnValidate()
        {
            Assert.IsTrue(_conditionContainer.type.IsSubclassOf(typeof(CharacterCondition)), $"The condition must be inherited from {nameof(CharacterCondition)}");
        }
        
        protected void InitializeCondition(Type condition)
        {
            _conditionContainer = new ConditionContainer(_conditionName, condition, _effects, _timeSec);
        }
    }
}