using Behaviour.Behaviours;
using Character;
using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace Behaviour
{
    public class BehaviourFactoryPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        private CharacterBehaviourPm.Ctx _behaviourCtx;

        public struct Ctx
        {
            public Animator animatorView;
            public Rigidbody rigidbody;
            public CharacterStats stats;
            public Transform characterTransform;
            public OrientationAxises orientationAxises;

            public ReactiveCommand<BehaviourInfo> onBehaviourAdded;
            public ReactiveCommand<CharacterBehaviourPm> onBehaviourCreated;
        }

        public BehaviourFactoryPm(Ctx ctx)
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
                animator = _ctx.animatorView,
                rigidbody = _ctx.rigidbody,
                characterTransform = _ctx.characterTransform,
                stats = _ctx.stats,
            };

            switch (behaviourInfo.configs.name)
            {
                default:
                case BehaviourName.Run:
                    behaviour = new RunBehaviourPm(_behaviourCtx);

                    break;
            }

            _ctx.onBehaviourCreated?.Execute(behaviour);
        }
    }
}