using Code.Player;
using Code.Session;

namespace Code.PlatformsBehaviour.Abstract
{
    public abstract class PlatformInteractingBehaviour
    {
        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public SessionListener sessionListener;
            public PlayerEntity playerEntity;
        }

        protected PlatformInteractingBehaviour(Ctx ctx)
        {
            _ctx = ctx;
        }

        public abstract void InteractWithPlayer();
    }
}