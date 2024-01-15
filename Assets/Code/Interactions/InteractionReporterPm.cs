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
    /// Распознает тип объекта взаимодействия и сообщает об этом в событиях
    /// принимает в себя абстрактное "сырье" в виде коллайдеров
    /// </summary>
    public class InteractionReporterPm : BaseDisposable
    {
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public Dictionary<LayerMask, LayerName> layersDictionary;

            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<GameObject> onInteractedWithObstacle;
            public ReactiveCommand<GameObject> onInteractedWithSaveZone;
            public ReactiveTrigger onFinish;
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

            var gameObject = collider.gameObject;
            var layer = gameObject.layer;

            if (_ctx.layersDictionary.TryGetValue(layer, out var name))
            {
                switch (name)
                {
                    case LayerName.Obstacle:
                        _ctx.onInteractedWithObstacle?.Execute(gameObject);

                        break;

                    case LayerName.SaveZone:

                        _ctx.onInteractedWithSaveZone?.Execute(gameObject);

                        break;

                    case LayerName.Finish:
                        _ctx.onFinish?.Notify();

                        break;
                }
            }
        }
    }
}