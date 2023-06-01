using Code.Configs;
using Code.Platforms.Abstract;
using UnityEngine;

namespace Code.Player
{
    public class PlayerStats
    {
        public float initialSpeed { get; private set; }
        public float currentSpeed { get; private set; }
        public float jumpForce { get; private set; }
        public float valueYToFallOut { get; private set; }

        public int maxJumpingTimes { get; private set; }
        public float immuneValue;
        public float resumeSpeedValue;

        public int currentLives { get; private set; }

        private readonly Ctx _ctx;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public Platform playerSpawnPoint;
        }

        public PlayerStats(Ctx ctx)
        {
            _ctx = ctx;

            currentLives = _ctx.playersConfigs.lives;
            maxJumpingTimes = _ctx.playersConfigs.maxJumpingTimes;
            initialSpeed = currentSpeed = _ctx.playersConfigs.initialSpeed;
            jumpForce = _ctx.playersConfigs.jumpForce;
            valueYToFallOut = _ctx.playerSpawnPoint.transform.position.y - 2f;
        }

        public void AddLives(int value)
        {
            value = Mathf.Abs(value);

            currentLives += value;
        }

        public void RemoveLifes(int value)
        {
            value = Mathf.Abs(value);
            currentLives -= value;
        }

        public void ChangeSpeed(float value)
        {
            currentSpeed += value;
        }
        
        public void SetSpeed(float value)
        {
            currentSpeed = value;
        }

        public void SetDefaultSpeed()
        {
            currentSpeed = initialSpeed;
        }

        public void AddImmune(float value)
        {
            value = Mathf.Abs(value);
            immuneValue = value;
        }
    }
}