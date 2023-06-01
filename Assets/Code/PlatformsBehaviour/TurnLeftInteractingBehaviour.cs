using Code.Platforms.Abstract;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;

namespace Code.PlatformsBehaviour
{
    public class TurnLeftInteractingBehaviour : PlatformInteractingBehaviour
    {
        private readonly PlayerController _player;

        public TurnLeftInteractingBehaviour(PlayerController player)
        {
            _player = player;
        }

        public override void InteractWithPlayer(Platform platform)
        {
            _player.RotateLeft();
        }
    }
}