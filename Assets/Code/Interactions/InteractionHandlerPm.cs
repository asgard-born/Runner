using Framework;
using Items;
using Items.Behavioural;
using Items.Conditional;
using Shared;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionHandlerPm : BaseDisposable
    {
        public ReactiveCommand<BehaviourContainer> _onBehaviourChanged;
        public ReactiveCommand<ConditionContainer> _onConditionAdded;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourContainer> onBehaviourChanged;
            public ReactiveCommand<ConditionContainer> onConditionAdded;
        }

        public InteractionHandlerPm(Ctx ctx)
        {
            _onBehaviourChanged = ctx.onBehaviourChanged;
            _onConditionAdded = ctx.onConditionAdded;
            AddUnsafe(ctx.onInterraction.Subscribe(Handle));
        }

        private void Handle(Collider collider)
        {
            if (collider == null)
            {
                Debug.LogError($"The collider can't be null");

                return;
            }

            var item = collider.GetComponent<Item>();

            if (item != null)
            {
                switch (item)
                {
                    case BehaviouralItem behaviouralItem:
                        _onBehaviourChanged?.Execute(behaviouralItem.behaviourContainer);

                        break;

                    case ConditionalItem conditionalItem:
                        _onConditionAdded?.Execute(conditionalItem.conditionContainer);

                        break;
                }
            }
        }
    }
}