using System;
using Shared;
using UnityEngine;

namespace Items.Conditional
{
    public abstract class ConditionalItem : Item
    {
        [SerializeField] protected ConditionName _conditionName;
        
        protected ConditionContainer _conditionContainer;

        public ConditionContainer conditionContainer => _conditionContainer;

        protected void InitializeCondition(Type condition)
        {
            _conditionContainer = new ConditionContainer(_conditionName, condition, _effects, _timeSec);
        }
    }
}