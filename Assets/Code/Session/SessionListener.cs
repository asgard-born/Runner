﻿using System;
using System.Collections.Concurrent;
using Code.Configs;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Session
{
    public class SessionListener
    {
        public Action<ConcurrentDictionary<PlatformType, int>> onWin;
        public float spawnPlatformsDelaySec;

        private readonly Ctx _ctx;
        private int _passedPlatformsCount;
        private ConcurrentDictionary<PlatformType, int> _passedPlatformsDictionary = new ConcurrentDictionary<PlatformType, int>();

        public struct Ctx
        {
            public LevelConfigs levelConfigs;
        }

        public SessionListener(Ctx ctx)
        {
            _ctx = ctx;
            spawnPlatformsDelaySec = _ctx.levelConfigs.spawnPlatformsDelaySec;
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

        public void OnSpeedChanged(float previousValue, float factor)
        {
            var newValue = previousValue + factor;
            var proportion = previousValue / newValue;
            spawnPlatformsDelaySec *= proportion;
        }
    }
}