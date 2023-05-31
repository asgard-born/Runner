using System;
using System.Collections.Concurrent;
using Code.Configs;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Session
{
    public class SessionListener
    {
        public float initialSpeed { get; private set; }
        public float currentSpeed { get; private set; }
        public float jumpForce { get; private set; }
        public int maxJumpingTimes { get; private set; }
        public float valueYToFallOut { get; private set; }

        public Action<ConcurrentDictionary<PlatformType, int>> onWin;

        private readonly Ctx _ctx;
        private int _passedPlatformsCount;
        private ConcurrentDictionary<PlatformType, int> _passedPlatformsDictionary = new ConcurrentDictionary<PlatformType, int>();

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public LevelConfigs levelConfigs;
        }

        public SessionListener(Ctx ctx)
        {
            _ctx = ctx;
            
            initialSpeed = currentSpeed = _ctx.playersConfigs.initialSpeed;
            jumpForce = _ctx.playersConfigs.jumpForce;
            maxJumpingTimes = _ctx.playersConfigs.maxJumpingTimes;
        }
        
        public void AddPassedPlatform(PlatformType platformType)
        {
            _passedPlatformsDictionary.AddOrUpdate(platformType, 1, (id, count) => count + 1);
            _passedPlatformsCount++;
            
            Debug.Log($"PASSED: {platformType}, all {_passedPlatformsCount}");

            if (_passedPlatformsCount >= _ctx.levelConfigs.allPlatformsCount)
            {
                onWin?.Invoke(_passedPlatformsDictionary);
            }
        }

        public void SetYValueToFallOut(float value)
        {
            valueYToFallOut = value;
        }
        
        public void AddSpeed(float factor)
        {
            currentSpeed *= factor;
        }

        public void ReduceSpeed(float factor)
        {
            currentSpeed /= factor;
        }
        
        public void SetDefaultSpeed()
        {
            currentSpeed = initialSpeed;
        }
    }
}