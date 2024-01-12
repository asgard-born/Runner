using Configs;
using Framework;
using Items;
using Obstacles;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionReporterService : BaseDisposable
    {
        private ReactiveCommand<BehaviourConfigs> _onBehaviourAdded;
        private ReactiveCommand<Obstacle> _onCrashIntoObstacle;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourConfigs> onBehaviourAdded;
            public ReactiveCommand<Obstacle> onCrashIntoObstacle;
        }

        public InteractionReporterService(Ctx ctx)
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
                foreach (var VARIABLE in item.behaviours)
                {
                    switch (item)
                    {
                  
                    }
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