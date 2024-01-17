using System;
using System.Linq.Expressions;
using Behaviour.Behaviours.Abstract;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Behaviour
{
    /// <summary>
    /// Данному классу делегируется создание объектов поведений и их отправка в реактивных командах.
    /// Создает поведение по типу, получаемому из конфигов
    /// </summary>
    public class CharacterBehaviourFactoryPm : BaseDisposable
    {
        private readonly Ctx _ctx;

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
            public ReactiveTrigger<BehaviourKey, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourKey> onBehaviourAdded;
            public ReactiveCommand<BehaviourKey> onBehaviourFinished;
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
            var ctx = new CharacterBehaviourPm.Ctx
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

            Type type = behaviourInfo.configs.type;
            
            var ctxParam = Expression.Parameter(typeof(CharacterBehaviourPm.Ctx), "ctx");
            var newExpression = Expression.New(type.GetConstructor(new[] { typeof(CharacterBehaviourPm.Ctx) }), ctxParam);
            
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(CharacterBehaviourPm.Ctx), type);
            var lambda = Expression.Lambda(delegateType, newExpression, ctxParam); 
            
            var createInstance = (Func<CharacterBehaviourPm.Ctx, CharacterBehaviourPm>)lambda.Compile();
            var instance = createInstance(ctx);
            
            _ctx.onNewBehaviourProduced?.Notify(behaviourInfo.configs.key, instance);
        }
    }
}