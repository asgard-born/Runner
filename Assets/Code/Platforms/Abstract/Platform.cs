using Code.ObjectsPool;
using Code.Platforms.Essences;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject
    {
        public PlatformType platformType;
    }
}