using Behaviour.Behaviours;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Behaviour
{
    public class CharacterBehaviourFactoryPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        private CharacterBehaviourPm.Ctx _behaviourCtx;

        public struct Ctx
        {
            public CharacterState state;
            public Animator animator;
            public Rigidbody rigidbody;
            public Transform characterTransform;
            public float toleranceSideDistance;

            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveTrigger<BehaviourType, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourType> onBehaviourAdded;
            public ReactiveCommand<BehaviourType> onBehaviourFinished;
        }

        public CharacterBehaviourFactoryPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(ctx.onBehaviourTaken.Subscribe(Create));
        }

        private void Create(BehaviourInfo behaviourInfo)
        {
            CharacterBehaviourPm behaviour;

            _behaviourCtx = new CharacterBehaviourPm.Ctx
            {
                configs = behaviourInfo.configs,
                isEndless = behaviourInfo.isEndless,
                durationSec = behaviourInfo.durationSec,
                animator = _ctx.animator,
                rigidbody = _ctx.rigidbody,
                characterTransform = _ctx.characterTransform,
                toleranceSideDistance = _ctx.toleranceSideDistance,
                state = _ctx.state,
                
                onSwipeDirection = _ctx.onSwipeDirection,
                onBehaviourAdded = _ctx.onBehaviourAdded,
                onBehaviourFinished = _ctx.onBehaviourFinished
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

            _ctx.onNewBehaviourProduced?.Notify(behaviourInfo.configs.type, behaviour);
        }
    }
}