using System.Collections.Generic;
using Framework;
using Framework.Reactive;
using Items;
using Shared;
using UniRx;
using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Принимает в себя 'сырые' данные в виде коллайдеров
    /// Распознает объект взаимодействия и сообщает об этом в событиях
    /// </summary>
    public class InteractionHandlerPm : BaseDisposable
    {
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public Dictionary<LayerMask, LayerName> layersDictionary;

            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<GameObject> onInteractWithObstacle;
            public ReactiveCommand<Transform> onInteractWithSaveZone;
            public ReactiveTrigger onFinishReached;
            public ReactiveTrigger onCoinTaken;
        }

        public InteractionHandlerPm(Ctx ctx)
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

            var gameObject = collider.gameObject;
            var layer = gameObject.layer;

            foreach (var layerMaskPair in _ctx.layersDictionary)
            {
                if ((layerMaskPair.Key.value & (1 << layer)) != 0)
                {
                    switch (layerMaskPair.Value)
                    {
                        case LayerName.Obstacle:
                            _ctx.onInteractWithObstacle?.Execute(gameObject);

                            break;

                        case LayerName.SavePoint:
                            _ctx.onInteractWithSaveZone?.Execute(gameObject.transform);

                            break;

                        case LayerName.Finish:
                            _ctx.onFinishReached?.Notify();

                            break;
                    }

                    return;
                }
            }

            var item = collider.GetComponent<Item>();

            if (item != null)
            {
                foreach (var behaviourInfo in item.behaviours)
                {
                    _onBehaviourTaken?.Execute(behaviourInfo);
                }

                if (item.isCoin)
                {
                    _ctx.onCoinTaken.Notify();
                }

                item.gameObject.SetActive(false);
            }
        }
    }
}