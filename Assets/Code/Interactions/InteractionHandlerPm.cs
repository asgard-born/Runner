using Framework;
using UniRx;
using UnityEngine;

namespace Interactions
{
    public class InteractionHandlerPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
        }

        public InteractionHandlerPm(Ctx ctx)
        {
            AddUnsafe(ctx.onInterraction.Subscribe(Handle));
        }

        private void Handle(Collider gameObject)
        {
            
        }
    }
}