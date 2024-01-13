using System.Collections.Generic;
using Framework;
using Shared;
using UniRx;
using UnityEngine;

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

        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public float durationSec;
            public GameObject[] effects;
            public Animator animator;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public CharacterState state;
            public LinkedList<Roadline> roadlines;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
        }

        public CharacterBehaviourPm(Ctx ctx)
        {
            _ctx = ctx;
        }

        protected abstract void DoBehave();
        protected abstract void OnSwipe(SwipeDirection swipeDirection);
    }
}