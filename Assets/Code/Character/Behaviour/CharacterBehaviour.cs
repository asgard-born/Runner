using Framework;
using Shared;
using Shared.Containers;
using UnityEngine;

namespace Character.Behaviour
{
    public abstract class CharacterBehaviour : BaseDisposable
    {
        protected bool _isMoving;
        protected CharacterAction _currentAction;

        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public float time;
            public EffectContainer[] effects;
            public CharacterAnimatorView animatorView;
            public Rigidbody rigidbody;
            public Transform transform;
            public CharacterStats stats;
        }

        public CharacterBehaviour(Ctx ctx)
        {
            _ctx = ctx;
        }

        public abstract void DoBehave();
        public abstract void OnSwipe(SwipeDirection swipeDirection);
    }
}