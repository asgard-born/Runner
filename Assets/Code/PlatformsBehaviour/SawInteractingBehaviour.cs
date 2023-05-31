using Code.PlatformsBehaviour.Abstract;
using Code.Player;

namespace Code.PlatformsBehaviour
{
    public class SawInteractingBehaviour : PlatformInteractingBehaviour
    {
        private readonly PlayerController _player;

        public SawInteractingBehaviour(PlayerController player)
        {
            _player = player;
        }

        public override void InteractWithPlayer()
        {
            _player.Hit();
        }
    }
}