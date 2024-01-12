using System;
using Character.Behaviour;
using Shared;
using UnityEngine.Assertions;

namespace Items.Behavioural
{
    public abstract class BehaviouralItem : Item
    {
        protected BehaviourContainer _behaviourContainer;

        public BehaviourContainer behaviourContainer => _behaviourContainer;

        private void Start()
        {
            Assert.IsTrue(_behaviourContainer.type.IsSubclassOf(typeof(CharacterBehaviour)), $"The behaviour must be inherited from {nameof(CharacterBehaviour)}");
        }

        protected void InitializeBehaviour(Type behaviour)
        {
            _behaviourContainer = new BehaviourContainer(behaviour, _effects, _timeSec);
        }
    }
}