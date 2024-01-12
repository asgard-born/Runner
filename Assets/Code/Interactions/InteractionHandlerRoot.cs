using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionHandlerRoot : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<BehaviourContainer> onBehaviourChanged;
            public ReactiveCommand<ConditionContainer> onConditionAdded;
            public ReactiveCommand<GameObject> onCrashIntoObstacle;
        }

        public InteractionHandlerRoot(Ctx ctx)
        {
            var pmCtx = new InteractionHandlerPm.Ctx
            {
                onInterraction = ctx.onInterraction
            };

            AddUnsafe(new InteractionHandlerPm(pmCtx));
        }
    }
}