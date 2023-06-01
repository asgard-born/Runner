using Code.Platforms.Abstract;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;

namespace Code.PlatformsBehaviour
{
    public class TurnRightInteractingBehaviour : PlatformInteractingBehaviour
    {
        private readonly PlayerController _player;

        public TurnRightInteractingBehaviour(PlayerController player)
        {
            _player = player;
        }

        public override void InteractWithPlayer(Platform platform)
        {
            _player.RotateRight();
        }
    }
}