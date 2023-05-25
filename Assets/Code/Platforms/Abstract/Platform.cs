using Code.ObjectsPool;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject
    {
        public Transform neighbourPlatform;
        public PlatformType platformType;
    }
}