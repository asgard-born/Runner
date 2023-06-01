using System.Collections.Concurrent;
using Code.Configs;
using Code.Platforms.Essences;

namespace Code.Session
{
    public class SessionListener
    {
        public float spawnPlatformsDelaySec;

        private readonly Ctx _ctx;
        private ConcurrentDictionary<PlatformType, int> _passedPlatformsDictionary = new ConcurrentDictionary<PlatformType, int>();

        public struct Ctx
        {
            public ConcurrentDictionary<PlatformType, int> passedPlatformsDictionary;
            public LevelConfigs levelConfigs;
        }

        public SessionListener(Ctx ctx)
        {
            _ctx = ctx;
            _passedPlatformsDictionary = _ctx.passedPlatformsDictionary;
            spawnPlatformsDelaySec = _ctx.levelConfigs.spawnPlatformsDelaySec;
        }

        public void AddPassedPlatform(PlatformType platformType)
        {
            _passedPlatformsDictionary.AddOrUpdate(platformType, 1, (id, count) => count + 1);
        }

        public void OnSpeedChanged(float previousValue, float factor)
        {
            var newValue = previousValue + factor;
            var proportion = previousValue / newValue;
            spawnPlatformsDelaySec *= proportion;
        }
    }
}