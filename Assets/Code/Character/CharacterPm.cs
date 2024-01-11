using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Character.Behaviour;
using Character.Conditions;
using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace Character
{
    public class CharacterPm : BaseDisposable
    {
        private CharacterBehaviour _behaviour;
        private Dictionary<ConditionName, CharacterCondition> _conditions = new();

        private Ctx _ctx;
        private CharacterBehaviour.Ctx _behaviourCtx;

        public struct Ctx
        {
            public CharacterAnimatorView animatorView;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public Transform spawnPoint;
            public CharacterStats stats;
            public ReactiveCommand<BehaviourContainer> onBehaviourChanged;
            public ReactiveCommand<ConditionContainer> onConditionAdded;
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

            AddUnsafe(Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                DoBehave();
                DoConditioning();
            }));
        }

        private void DoConditioning()
        {
            foreach (var condition in _conditions.Values)
            {
                condition.DoConditioning();
            }
        }

        private void InitializeCharacter()
        {
            _ctx.characterTransform.position = _ctx.spawnPoint.position;
            _ctx.characterTransform.rotation = _ctx.spawnPoint.rotation;

            _behaviour = new RunBehaviour(_behaviourCtx);
            
            _ctx.onCharacterInitialized?.Execute(_ctx.characterTransform);
        }

        private void InitializeRx(Ctx ctx)
        {
            AddUnsafe(ctx.onBehaviourChanged.Subscribe(onBehaviourChanged));
            AddUnsafe(ctx.onConditionAdded.Subscribe(OnConditionAdded));
        }

        private void DoBehave()
        {
            _behaviour.DoBehave();
        }

        private void onBehaviourChanged(BehaviourContainer container)
        {
            if (!container.type.IsSubclassOf(typeof(CharacterBehaviour)))
            {
                Debug.LogError($"The condition must be inherited from {nameof(CharacterBehaviour)}");

                return;
            }

            ParameterExpression ctxParam = Expression.Parameter(typeof(CharacterBehaviour.Ctx), "ctx");
            NewExpression newExpression = Expression.New(typeof(CharacterBehaviour).GetConstructor(new[] { typeof(CharacterBehaviour.Ctx) }), ctxParam);
            LambdaExpression lambda = Expression.Lambda(container.type, newExpression);
            Func<CharacterBehaviour.Ctx, CharacterBehaviour> createInstance = (Func<CharacterBehaviour.Ctx, CharacterBehaviour>)lambda.Compile();

            var ctx = new CharacterBehaviour.Ctx
            {
                time = container.timeSec,
                effects = container.effects,
                animatorView = _ctx.animatorView,
                rigidbody = _ctx.rigidbody,
                transform = _ctx.characterTransform,
                stats = _ctx.stats
            };
            
            CharacterBehaviour instance = createInstance(ctx);

            _behaviour.Dispose();
            _behaviour = instance;
        }

        private void OnConditionAdded(ConditionContainer container)
        {
            if (container.name == ConditionName.None)
            {
                Debug.LogError("The condition must be defined");

                return;
            }

            if (!container.type.IsSubclassOf(typeof(CharacterCondition)))
            {
                Debug.LogError($"The condition must be inherited from {nameof(CharacterCondition)}");

                return;
            }

            ParameterExpression ctxParam = Expression.Parameter(typeof(CharacterCondition.Ctx), "ctx");
            NewExpression newExpression = Expression.New(typeof(CharacterCondition).GetConstructor(new[] { typeof(CharacterCondition.Ctx) }), ctxParam);
            LambdaExpression lambda = Expression.Lambda(container.type, newExpression);
            Func<CharacterCondition.Ctx, CharacterCondition> createInstance = (Func<CharacterCondition.Ctx, CharacterCondition>)lambda.Compile();

            var ctx = new CharacterCondition.Ctx
            {
                time = container.timeSec,
                effects = container.effects,
                animatorView = _ctx.animatorView,
                stats = _ctx.stats
            };
            
            CharacterCondition instance = createInstance(ctx);

            _conditions.Add(container.name, instance);
        }
    }
}