using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Configs;
using Code.Platforms.Abstract;
using Cysharp.Threading.Tasks;

namespace Code.Platforms
{
    public class PlatformsDestroyingSystem
    {
        private readonly LinkedList<Platform> _platforms;
        private readonly LevelConfigs _levelConfigs;
        private bool _hasStarted;

        public struct Ctx
        {
            public LinkedList<Platform> platforms;
            public LevelConfigs levelConfigs;
        }

        public PlatformsDestroyingSystem(Ctx ctx)
        {
            _platforms = ctx.platforms;
            _levelConfigs = ctx.levelConfigs;
        }

        public async void StartDestroyingCycleAsync()
        {
            if (_hasStarted) return;

            _hasStarted = true;

            while (true)
            {
                if (_platforms.Count > _levelConfigs.maxPlatformsInTime)
                {
                    var firstPlatform = _platforms.First.Value;
                    firstPlatform.ReturnToPool();
                    firstPlatform.Dispose();
                    _platforms.RemoveFirst();
                }

                await UniTask.Delay((int)(_levelConfigs.destroyPlatformsDelaySec * 1000));
            }
        }
    }
}