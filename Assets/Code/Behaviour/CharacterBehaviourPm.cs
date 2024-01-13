using System;
using System.Collections.Generic;
using Framework;
using Shared;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Behaviour
{
    /// <summary>
    /// Базовый класс для реализации различных видов поведения персонажа
    /// Реализует собой паттерн 'Стратегия': мы делегируем поведению управление персонажем
    /// Для сохранения слабой связанности сюда не передается view напрямую
    /// </summary>
    public abstract class CharacterBehaviourPm : BaseDisposable
    {
        protected bool _isMoving;
        protected CharacterAction _currentAction;
        protected float _secondsLeft;
        protected GameObject[] _spawnedEffects;

        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public BehaviourType type;
            public bool isEndless;
            public float durationSec;
            public GameObject[] effects;
            public Animator animator;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public CharacterState state;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveCommand<BehaviourType> onBehaviourAdded;
            public ReactiveCommand<BehaviourType> onBehaviourFinished;
        }

        protected CharacterBehaviourPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(_ctx.onSwipeDirection.Subscribe(OnSwipeDirection));
            AddUnsafe(_ctx.onBehaviourAdded.Subscribe(OnBehaviourAdded));
        }

        private void OnBehaviourAdded(BehaviourType type)
        {
            if (type != _ctx.type) return;

            _secondsLeft = _ctx.durationSec;

            if (_ctx.effects != null)
            {
                _spawnedEffects = new GameObject[_ctx.effects.Length];

                for (var i = 0; i < _ctx.effects.Length; i++)
                {
                    _spawnedEffects[i] = Object.Instantiate(_ctx.effects[i], _ctx.characterTransform.position, _ctx.characterTransform.rotation, _ctx.characterTransform);
                }
            }

            AddUnsafe(Observable.EveryFixedUpdate().Subscribe(_ => DoBehaveProcess()));
        }

        private void DoBehaveProcess()
        {
            DoBehave();

            if (!_ctx.isEndless)
            {
                _secondsLeft -= Time.fixedDeltaTime;

                if (_secondsLeft <= 0)
                {
                    _ctx.onBehaviourFinished?.Execute(_ctx.type);
                }
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            foreach (var effect in _spawnedEffects)
            {
                Object.Destroy(effect);
            }

            _spawnedEffects = null;
        }

        protected abstract void DoBehave();
        protected abstract void OnSwipeDirection(SwipeDirection swipeDirection);
    }
}