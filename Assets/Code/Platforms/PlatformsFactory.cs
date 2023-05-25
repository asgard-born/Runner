using System.Collections.Generic;
using Code.Configs;
using Code.ObjectsPool;
using Code.Platforms.Abstract;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsFactory : MonoBehaviour
    {
        private Ctx _ctx;
        private LinkedList<Platform> _platforms;

        public struct Ctx
        {
            public GameConfigs gameConfigs;
            public PoolsConfigs poolsConfigs;
            public Platform lastPlatform;
        }

        private PlatformsFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        private T CreatePlatform<T>(Vector3 position, Quaternion rotation) where T : Platform
        {
            if (_platforms.Count >= _ctx.gameConfigs.maxPlatformsCount)
            {
                _platforms.RemoveFirst();
            }

            var platform = PoolsManager.GetObject<T>(position, rotation);

            _platforms.AddLast(platform);

            return platform;
        }
    }
}