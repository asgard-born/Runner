﻿using Code.Platforms.Abstract;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;

namespace Code.PlatformsBehaviour
{
    public class FenceInteractingBehaviour : PlatformInteractingBehaviour
    {
        private readonly PlayerController _player;

        public FenceInteractingBehaviour(PlayerController player)
        {
            _player = player;
        }

        public override void InteractWithPlayer(Platform platform)
        {
            _player.TryHit();
        }
    }
}