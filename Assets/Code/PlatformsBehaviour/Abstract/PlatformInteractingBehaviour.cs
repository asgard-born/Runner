using Code.Player;

namespace Code.PlatformsBehaviour.Abstract
{
    public abstract class PlatformInteractingBehaviour
    {
        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public SessionStats sessionStats;
            public PlayerEntity playerEntity;
        }

        protected PlatformInteractingBehaviour(Ctx ctx)
        {
            _ctx = ctx;
        }

        public abstract void InteractWithPlayer();
    }
}