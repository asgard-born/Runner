using Code.PlatformsBehaviour.Abstract;
using Code.Player;
using Code.UI.Screens;

namespace Code.PlatformsBehaviour
{
    public class SawInteractingBehaviour : PlatformInteractingBehaviour
    {
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public PlayerController player;
            public HUDScreen hudScreen;
        }

        public SawInteractingBehaviour(Ctx ctx)
        {
            _ctx = ctx;
        }

        public override void InteractWithPlayer()
        {
            _ctx.hudScreen.RemoveLife();
            _ctx.player.Hit();
        }
    }
}