﻿using System;
using Configs;
using Framework;
using Shared;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Behaviour.Behaviours.Abstract
{
    /// <summary>
    /// Базовый класс для реализации различных видов поведения персонажа
    /// Реализует собой паттерн 'Стратегия': мы делегируем поведению управление персонажем
    /// Для сохранения слабой связанности в контекст класса не передается view напрямую
    /// </summary>
    public abstract class CharacterBehaviourPm : BaseDisposable
    {
        protected float _secondsLeft;
        protected GameObject[] _spawnedEffects;

        protected Ctx _ctx;

        public struct Ctx
        {
            public BehaviourConfigs configs;
            public bool isEndless;
            public float durationSec;
            public Animator animator;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public float toleranceSideDistance;
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
            if (type != _ctx.configs.type) return;

            _secondsLeft = _ctx.durationSec;
            var effects = _ctx.configs.effects;

            if (effects != null)
            {
                _spawnedEffects = new GameObject[effects.Length];

                for (var i = 0; i < effects.Length; i++)
                {
                    _spawnedEffects[i] = Object.Instantiate(effects[i], _ctx.characterTransform.position, _ctx.characterTransform.rotation, _ctx.characterTransform);
                }
            }

            AddUnsafe(Observable.EveryFixedUpdate().Subscribe(_ => DoBehaveProcess()));
        }

        private void DoBehaveProcess()
        {
            Behave();

            // Учитываем, что любое потенциальное поведение может выть временным или даваться на постоянной основе
            if (!_ctx.isEndless)
            {
                _secondsLeft -= Time.fixedDeltaTime;

                if (_secondsLeft <= 0)
                {
                    OnTimesOver();
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
        
        protected abstract void OnTimesOver();
        protected abstract void InitializeState();
        protected abstract void Behave();
        protected abstract void OnSwipeDirection(SwipeDirection swipeDirection);
    }
}