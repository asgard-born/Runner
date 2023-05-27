using System.Collections.Concurrent;
using Code.Platforms.Essences;

namespace Code
{
    public class SessionStats
    {
        public ConcurrentDictionary<PlatformType, int> platformsDictionary = new ConcurrentDictionary<PlatformType, int>();

        public void AddPlatformCount(PlatformType platformType)
        {
            platformsDictionary.AddOrUpdate(platformType, 1, (id, count) => count + 1);
        }
    }
}