using Framework;
using Items;
using Obstacles;
using Shared;
using UniRx;
using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Распознает тип объекта взаимодействия и сообщает об этом в событиях
    /// принимает в себя абстрактное "сырье" в виде коллайдеров
    /// </summary>
    public class InteractionReporterPm : BaseDisposable
    {
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<Obstacle> onInteractedWithObstacle;
        }

        public InteractionReporterPm(Ctx ctx)
        {
            _ctx = ctx;
            _onBehaviourTaken = ctx.onBehaviourTaken;
            
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
                    _onBehaviourTaken?.Execute(behaviourInfo);
                }
                
                item.gameObject.SetActive(false);
                return;
            }
            
            var obstacle = collider.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                _ctx.onInteractedWithObstacle?.Execute(obstacle);
            }
        }
    }
}