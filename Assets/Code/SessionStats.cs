using System.Collections.Concurrent;
using Code.Configs;
using Code.Platforms.Essences;

namespace Code
{
    public class SessionStats
    {
        public ConcurrentDictionary<PlatformType, int> platformsDictionary = new ConcurrentDictionary<PlatformType, int>();
        private readonly Ctx _ctx;
        public float currentSpeed { get; private set; }
        public float jumpForce { get; private set; }

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public LevelConfigs levelConfigs;
        }

        public SessionStats(Ctx ctx)
        {
            _ctx = ctx;
            currentSpeed = _ctx.playersConfigs.initialSpeed;
            jumpForce = _ctx.playersConfigs.jumpForce;
        }
        
        public void AddPlatformCount(PlatformType platformType)
        {
            platformsDictionary.AddOrUpdate(platformType, 1, (id, count) => count + 1);
        }
        
        public void AddSpeed(float factor)
        {
            currentSpeed *= factor;
        }

        public void ReduceSpeed(float factor)
        {
            currentSpeed /= factor;
        }
    }
}