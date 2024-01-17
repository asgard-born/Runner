using Configs;
using Framework;
using Framework.Reactive;
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
        protected bool _canTiming;

        public struct Ctx
        {
            public BehaviourConfigs configs;
            public bool isEndless;
            public float behaviourDurationSec;
            public Animator animator;
            public Rigidbody rigidbody;
            public Collider collider;
            public LayerMask landingMask;
            public Transform transform;
            public Vector2 toleranceDistance;
            public CharacterState state;
            public float crashDelay;

            public ReactiveCommand<Direction> onSwipeDirection;
            public ReactiveCommand<BehaviourKey> onBehaviourAdded;
            public ReactiveCommand<BehaviourKey> onBehaviourFinished;
            public ReactiveCommand<GameObject> onInteractedWIthObstacle;
            public ReactiveTrigger onFinishZoneReached;
            public ReactiveCommand<Transform> onInteractWithSaveZone;
            public ReactiveTrigger onRespawned;
        }

        protected CharacterBehaviourPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddUnsafe(_ctx.onBehaviourAdded.Subscribe(OnBehaviourAdded));
        }

        private void OnBehaviourAdded(BehaviourKey key)
        {
            if (key != _ctx.configs.key) return;

            _secondsLeft = _ctx.behaviourDurationSec;
            var effects = _ctx.configs.effects;

            if (effects != null)
            {
                _spawnedEffects = new GameObject[effects.Length];

                for (var i = 0; i < effects.Length; i++)
                {
                    _spawnedEffects[i] = Object.Instantiate(effects[i], _ctx.transform);
                    _spawnedEffects[i].transform.rotation = _ctx.transform.rotation;
                }
            }

            // Учитываем, что любое потенциальное поведение может выть временным
            // или даваться на постоянной основе, поэтому пишем обработку в базовом классе
            if (!_ctx.isEndless)
            {
                AddUnsafe(Observable.EveryFixedUpdate().Subscribe(_ => DoTiming()));
            }
        }

        private void DoTiming()
        {
            if (!_canTiming) return;

            _secondsLeft -= Time.fixedDeltaTime;

            if (_secondsLeft <= 0)
            {
                OnTimeOver();
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

        protected abstract void Reset();
        protected abstract void OnTimeOver();
        protected abstract void Initialize();
    }
}