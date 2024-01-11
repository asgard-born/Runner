using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Character.Behaviour;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Character
{
    public class CharacterPm : BaseDisposable
    {
        private Dictionary<BehaviourType, CharacterBehaviour> _behaviours = new();

        private Ctx _ctx;
        private CharacterBehaviour.Ctx _behaviourCtx;

        public struct Ctx
        {
            public CharacterAnimatorView animatorView;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public Transform spawnPoint;
            public CharacterStats stats;
            public ReactiveEvent<BehaviourType, Type> onBehaviourAdded;
            public ReactiveCommand<Transform> onCharacterInitialized;
        }

        public CharacterPm(Ctx ctx)
        {
            _ctx = ctx;

            _behaviourCtx = new CharacterBehaviour.Ctx
            {
                animatorView = ctx.animatorView,
                rigidbody = ctx.rigidbody,
                transform = ctx.characterTransform,
                stats = ctx.stats,
            };

            InitializeRx(ctx);
            InitializeCharacter();

            AddUnsafe(Observable.EveryFixedUpdate().Subscribe(_ => DoBehave()));
        }

        private void InitializeCharacter()
        {
            _ctx.characterTransform.position = _ctx.spawnPoint.position;
            _ctx.characterTransform.rotation = _ctx.spawnPoint.rotation;
            
            _behaviours.Add(BehaviourType.Move, new RunBehaviour(_behaviourCtx));

            _ctx.onCharacterInitialized?.Execute(_ctx.characterTransform);
        }

        private void InitializeRx(Ctx ctx)
        {
            AddUnsafe(ctx.onBehaviourAdded.SubscribeWithSkip(OnBehaviourAdded));
        }

        private void DoBehave()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.DoBehave();
            }
        }

        private void OnBehaviourAdded(BehaviourType behaviourType, Type behaviour)
        {
            if (behaviourType == BehaviourType.None)
            {
                Debug.LogError("The behaviour must be defined");

                return;
            }

            if (!behaviour.IsSubclassOf(typeof(CharacterBehaviour)))
            {
                Debug.LogError($"The behaviour must be inherit from {nameof(CharacterBehaviour)}");

                return;
            }

            ParameterExpression ctxParam = Expression.Parameter(typeof(CharacterBehaviour.Ctx), "ctx");
            NewExpression newExpression = Expression.New(typeof(CharacterBehaviour).GetConstructor(new[] { typeof(CharacterBehaviour.Ctx) }), ctxParam);
            LambdaExpression lambda = Expression.Lambda(behaviour, newExpression);

            Func<CharacterBehaviour.Ctx, CharacterBehaviour> createInstance = (Func<CharacterBehaviour.Ctx, CharacterBehaviour>)lambda.Compile();
            CharacterBehaviour instance = createInstance(_behaviourCtx);

            _behaviours.Add(behaviourType, instance);
        }
    }
}