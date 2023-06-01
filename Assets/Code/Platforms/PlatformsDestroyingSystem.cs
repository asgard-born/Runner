using System.Collections.Generic;
using Code.Configs;
using Code.Platforms.Abstract;
using Cysharp.Threading.Tasks;

namespace Code.Platforms
{
    public class PlatformsDestroyingSystem
    {
        private readonly LinkedList<Platform> _platforms;
        private readonly LevelConfigs _levelConfigs;
        private bool _canRemove = true;

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
            while (true)
            {
                if (!_canRemove)
                {
                    await UniTask.Delay(500);
                    continue;
                }

                if (_platforms.Count > _levelConfigs.maxPlatformsInTime)
                {
                    var firstPlatform = _platforms.First.Value;
                    firstPlatform.Dispose();
                    _platforms.RemoveFirst();
                }

                await UniTask.Delay((int)(_levelConfigs.destroyPlatformsDelaySec * 1000));
            }
        }

        public void Pause()
        {
            _canRemove = false;
        }

        public void Resume()
        {
            _canRemove = true;
        }
    }
}