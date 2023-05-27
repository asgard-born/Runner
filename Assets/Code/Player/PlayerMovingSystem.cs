using Code.Configs;

namespace Code.Player
{
    public class PlayerMovingSystem
    {
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public PlayerEntity player;
            public PlayersConfigs playersConfigs;
        }

        public PlayerMovingSystem(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}