using Code.PlatformsBehaviour.Abstract;

namespace Code.PlatformsBehaviour
{
    public class TurnLeftInteractingBehaviour : PlatformInteractingBehaviour
    {
        public TurnLeftInteractingBehaviour(Ctx ctx) : base(ctx)
        {
        }

        public override void InteractWithPlayer()
        {
            _ctx.playerEntity.RotateLeft();
        }
    }
}