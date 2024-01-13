using Configs;
using Framework;
using Items;
using Obstacles;
using Shared;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionReporter : BaseDisposable
    {
        private ReactiveCommand<BehaviourInfo> _onBehaviourAdded;
        private ReactiveCommand<Obstacle> _onCrashIntoObstacle;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourInfo> onBehaviourAdded;
            public ReactiveCommand<Obstacle> onCrashIntoObstacle;
        }

        public InteractionReporter(Ctx ctx)
        {
            _onBehaviourAdded = ctx.onBehaviourAdded;
            _onCrashIntoObstacle = ctx.onCrashIntoObstacle;
            
            AddUnsafe(ctx.onInterraction.Subscribe(TryReport));
        }

        private void TryReport(Collider collider)
        {
            if (collider == null)
            {
                Debug.LogError("The collider can't be null");

                return;
            }

            var item = collider.GetComponent<Item>();

            if (item != null)
            {
                foreach (var behaviourInfo in item.behaviours)
                {
                    _onBehaviourAdded?.Execute(behaviourInfo);
                }
                
                item.gameObject.SetActive(false);
                return;
            }
            
            var obstacle = collider.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                _onCrashIntoObstacle?.Execute(obstacle);
            }
        }
    }
}