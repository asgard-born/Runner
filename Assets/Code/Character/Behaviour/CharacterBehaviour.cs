using Framework;
using Shared;
using UnityEngine;

namespace Character.Behaviour
{
    public abstract class CharacterBehaviour : BaseDisposable
    {
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
        protected abstract void OnButtonUp();
        protected abstract void OnButtonDown();
        protected abstract void OnButtonLeft();
        protected abstract void OnButtonRight();
    }
}