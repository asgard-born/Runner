using Behaviour.Behaviours;
using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace Behaviour
{
    public class BehaviourFactory : BaseDisposable
    {
        private readonly Ctx _ctx;

        private CharacterBehaviourPm.Ctx _behaviourCtx;

        public struct Ctx
        {
            public CharacterState state;
            public Animator animator;
            public Rigidbody rigidbody;
            public Transform characterTransform;

            public ReactiveCommand<BehaviourInfo> onBehaviourAdded;
            public ReactiveCommand<CharacterBehaviourPm> onBehaviourCreated;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
        }

        public BehaviourFactory(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(ctx.onBehaviourAdded.Subscribe(Create));
        }

        private void Create(BehaviourInfo behaviourInfo)
        {
            CharacterBehaviourPm behaviour;

            _behaviourCtx = new CharacterBehaviourPm.Ctx
            {
                durationSec = behaviourInfo.durationSec,
                effects = behaviourInfo.configs.effects,
                animator = _ctx.animator,
                rigidbody = _ctx.rigidbody,
                characterTransform = _ctx.characterTransform,
                state = _ctx.state,
                onSwipeDirection = _ctx.onSwipeDirection
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

            _ctx.onBehaviourCreated?.Execute(behaviour);
        }
    }
}