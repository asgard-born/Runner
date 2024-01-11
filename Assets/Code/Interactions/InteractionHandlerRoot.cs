using Framework;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionHandlerRoot : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
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