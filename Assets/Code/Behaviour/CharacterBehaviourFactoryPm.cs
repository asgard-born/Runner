using Behaviour.Behaviours.Abstract;
using Behaviour.Behaviours.Moving;
using Behaviour.Behaviours.Velocity;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Behaviour
{
    /// <summary>
    /// Данному классу делегируется создание объектов поведений и их отправка в реактивных командах.
    /// Определяет нужное поведение от полученных кофигов по ключу и создает нужную реализацию
    /// </summary>
    public class CharacterBehaviourFactoryPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        private CharacterBehaviourPm.Ctx _behaviourCtx;

        public struct Ctx
        {
            public CharacterState state;
            public Animator animator;
            public Rigidbody rigidbody;
            public Collider collider;
            public LayerMask landingMask;
            public Transform characterTransform;
            public Vector2 toleranceDistance;
            public float crashDelay;

            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<Direction> onSwipeDirection;
            public ReactiveTrigger<BehaviourType, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourType> onBehaviourAdded;
            public ReactiveCommand<BehaviourType> onBehaviourFinished;
            public ReactiveCommand<GameObject> onInteractWithObstacle;
            public ReactiveTrigger onFinishZoneReached;
            public ReactiveCommand<Transform> onInteractWithSaveZone;
            public ReactiveTrigger onRespawned;
        }

        public CharacterBehaviourFactoryPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(ctx.onBehaviourTaken.Subscribe(Create));
        }

        private void Create(BehaviourInfo behaviourInfo)
        {
            CharacterBehaviourPm behaviour;

            _behaviourCtx = new CharacterBehaviourPm.Ctx
            {
                configs = behaviourInfo.configs,
                isEndless = behaviourInfo.isEndless,
                behaviourDurationSec = behaviourInfo.durationSec,
                animator = _ctx.animator,
                rigidbody = _ctx.rigidbody,
                collider = _ctx.collider,
                landingMask = _ctx.landingMask,
                transform = _ctx.characterTransform,
                toleranceDistance = _ctx.toleranceDistance,
                state = _ctx.state,
                crashDelay = _ctx.crashDelay,
                onRespawned = _ctx.onRespawned,

                onSwipeDirection = _ctx.onSwipeDirection,
                onBehaviourAdded = _ctx.onBehaviourAdded,
                onBehaviourFinished = _ctx.onBehaviourFinished,
                onInteractedWIthObstacle = _ctx.onInteractWithObstacle,
                onFinishZoneReached = _ctx.onFinishZoneReached,
                onInteractWithSaveZone = _ctx.onInteractWithSaveZone
            };

            switch (behaviourInfo.configs.name)
            {
                default:
                case BehaviourName.Run:
                    behaviour = new RunBehaviourPm(_behaviourCtx);

                    break;

                case BehaviourName.Fly:
                    behaviour = new FlyBehaviourPm(_behaviourCtx);

                    break;

                case BehaviourName.Slow:
                    behaviour = new SlowBehaviourPm(_behaviourCtx);

                    break;

                case BehaviourName.Fast:
                    behaviour = new FastBehaviourPm(_behaviourCtx);

                    break;
            }

            _ctx.onNewBehaviourProduced?.Notify(behaviourInfo.configs.type, behaviour);
        }
    }
}