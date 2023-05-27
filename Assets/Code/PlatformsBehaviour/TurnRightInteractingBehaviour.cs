using Code.PlatformsBehaviour.Abstract;

namespace Code.PlatformsBehaviour
{
    public class TurnRightInteractingBehaviour : PlatformInteractingBehaviour
    {
        public TurnRightInteractingBehaviour(Ctx ctx) : base(ctx)
        {
        }

        public override void InteractWithPlayer()
        {
            _ctx.playerEntity.RotateRight();
        }
    }
}