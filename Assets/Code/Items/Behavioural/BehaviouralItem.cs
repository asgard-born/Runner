using System;
using Shared.Containers;

namespace Items.Behavioural
{
    public abstract class BehaviouralItem : Item
    {
        protected BehaviourContainer _behaviourContainer;

        public BehaviourContainer behaviourContainer => _behaviourContainer;

        protected void InitializeBehaviour(Type behaviour)
        {
            _behaviourContainer = new BehaviourContainer(behaviour, _effects, _timeSec);
        }
    }
}