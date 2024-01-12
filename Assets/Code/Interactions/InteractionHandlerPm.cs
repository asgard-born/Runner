using Framework;
using Items;
using Items.Behavioural;
using Items.Conditional;
using Obstacles;
using Shared.Containers;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionHandlerPm : BaseDisposable
    {
        private ReactiveCommand<BehaviourContainer> _onBehaviourChanged;
        private ReactiveCommand<ConditionContainer> _onConditionAdded;
        private ReactiveCommand<GameObject> _onCrashIntoObstacle;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourContainer> onBehaviourChanged;
            public ReactiveCommand<ConditionContainer> onConditionAdded;
            public ReactiveCommand<GameObject> onCrashIntoObstacle;
        }

        public InteractionHandlerPm(Ctx ctx)
        {
            _onBehaviourChanged = ctx.onBehaviourChanged;
            _onConditionAdded = ctx.onConditionAdded;
            _onCrashIntoObstacle = ctx.onCrashIntoObstacle;
            
            AddUnsafe(ctx.onInterraction.Subscribe(Handle));
        }

        private void Handle(Collider collider)
        {
            if (collider == null)
            {
                Debug.LogError("The collider can't be null");

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
                
                // item.ReturnToPool();
                return;
            }
            
            var obstacle = collider.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                ;
            }
        }
    }
}